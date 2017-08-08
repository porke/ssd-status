using SSD_Status.Core.Model.Parsers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Management;

namespace SSD_Status.Core.Model
{
    public class SsdDrive
    {
        public string Name { get; set; }
        public double CapacityInGb { get; set; }
        public string SerialNo { get; set; }
        public string InterfaceType { get; set; }
        public string FirmwareVersion { get; set; }

        private IList<IRecordParser> _recordParsers = new List<IRecordParser>()
        {
            new WearLevellingParser(),
            new PowerOnHoursParser(),
            new WrittenGigabytesParser(),
            new PercentLifetimeLeftParser(),
            new PowerCycleCountParser()
        };

        public SmartDataEntry ReadSmartAttributes()
        {
            var searcher = new ManagementObjectSearcher($"Select * from Win32_DiskDrive Where Model={Name}")
            {
                Scope = new ManagementScope(@"\root\wmi"),
                Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictData")
            };
            
            Dictionary<byte, double> outputEntries = _recordParsers.ToDictionary(x => x.AttributeId, x => 0.0);
            foreach (ManagementObject data in searcher.Get())
            {
                uint length = (uint)data.Properties["Length"].Value;
                byte[] bytes = (byte[])data.Properties["VendorSpecific"].Value;
                for (int i = 0; i * 12 + 2 < length; ++i)
                {
                    byte id = bytes[i * 12 + 2];
                    foreach (var parser in _recordParsers)
                    {
                        if (parser.CanParse(id))
                        {
                            var record = parser.Parse(bytes, i);
                            outputEntries[id] = record;
                        }
                    }
                }
            }

            Func<Type, byte> findAttributeIdFromParserType = parserType => _recordParsers.First(x => x.GetType() == parserType).AttributeId;

            return new SmartDataEntry(DateTime.Now,
                                 outputEntries[findAttributeIdFromParserType(typeof(WrittenGigabytesParser))],
                                 (int)outputEntries[findAttributeIdFromParserType(typeof(PowerOnHoursParser))],
                                 (int)outputEntries[findAttributeIdFromParserType(typeof(PercentLifetimeLeftParser))],
                                 (int)outputEntries[findAttributeIdFromParserType(typeof(WearLevellingParser))],
                                 (int)outputEntries[findAttributeIdFromParserType(typeof(PowerCycleCountParser))]);
        }
    }
}
