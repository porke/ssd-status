using SSD_Status.Core.Model.Parsers;
using System;
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

            var outputEntries = new Dictionary<byte, double>();
            foreach (ManagementObject data in searcher.Get())
            {
                uint length = (uint)data.Properties["Length"].Value;
                byte[] bytes = (byte[])data.Properties["VendorSpecific"].Value;
                for (int i = 0; i * 12 + 2 < length; ++i)
                {
                    int id = bytes[i * 12 + 2];
                    foreach (var parser in _recordParsers)
                    {
                        if (parser.CanParse((byte)id))
                        {
                            var record = parser.Parse(bytes, i);
                            outputEntries.Add((byte)id, record);
                        }
                    }
                }
            }

            return new SmartDataEntry(DateTime.Now,
                                 outputEntries[WrittenGigabytesParser.AttributeId],
                                 (int)outputEntries[PowerOnHoursParser.AttributeId],
                                 (int)outputEntries[PercentLifetimeLeftParser.AttributeId],
                                 (int)outputEntries[WearLevellingParser.AttributeId],
                                 (int)outputEntries[PowerCycleCountParser.AttributeId]);
        }
    }
}
