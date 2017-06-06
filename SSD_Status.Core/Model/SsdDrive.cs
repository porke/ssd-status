using SSD_Status.Core.Model.Parsers;
using System;
using System.Collections.Generic;
using System.Management;

namespace SSD_Status.Core.Model
{
    public class SsdDrive
    {
        public string Name { get; private set; }
        public double CapacityInGb { get; private set; }

        private IList<IRecordParser> _recordParsers = new List<IRecordParser>()
        {
            new WearLevellingParser(),
            new PowerOnHoursParser(),
            new WrittenGigabytesParser(),
            new PercentLifetimeLeftParser()
        };

        public DataEntry ReadSmartAttributes()
        {
            var searcher = new ManagementObjectSearcher("Select * from Win32_DiskDrive")
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

            return new DataEntry(DateTime.Now,
                                 outputEntries[WrittenGigabytesParser.AttributeId],
                                 (int)outputEntries[PowerOnHoursParser.AttributeId],
                                 (int)outputEntries[PercentLifetimeLeftParser.AttributeId],
                                 (int)outputEntries[WearLevellingParser.AttributeId]);
        }

        public double CalculateHostWrittenGbPerDay(DataEntry startEntry, DataEntry endEntry)
        {
            int days = (endEntry.Timestamp - startEntry.Timestamp).Days;
            return (endEntry.HostWrittenGb - startEntry.HostWrittenGb) / days;
        }

        public double CalculatePowerOnHoursPerDay(DataEntry startEntry, DataEntry endEntry)
        {
            int days = (endEntry.Timestamp - startEntry.Timestamp).Days;
            return (endEntry.PowerOnHours - startEntry.PowerOnHours) / (double)days;
        }

        public double CalculateHostWrittenGbPerPowerOnHours(DataEntry startEntry, DataEntry endEntry)
        {
            int powerOnHourDiff = (endEntry.PowerOnHours - startEntry.PowerOnHours);
            return (endEntry.HostWrittenGb - startEntry.HostWrittenGb) / powerOnHourDiff;
        }

        public double CalculateWearLevellingPerDay(DataEntry startEntry, DataEntry endEntry)
        {
            int days = (endEntry.Timestamp - startEntry.Timestamp).Days;
            return (endEntry.WearLevellingCount - startEntry.WearLevellingCount) / (double)days;
        }
    }
}
