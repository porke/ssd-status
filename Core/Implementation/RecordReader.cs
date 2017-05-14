using System;
using System.Collections.Generic;
using SSD_Status.Core.Api;
using System.Management;
using SSD_Status.Core.Implementation.Parsers;
using System.Diagnostics;

namespace SSD_Status.Core.Implementation
{
    internal class RecordReader : IRecordReader
    {
        private IList<IRecordParser> _recordParsers = new List<IRecordParser>()
        {
            new WearLevellingParser(),
            new PowerOnHoursParser(),
            new WrittenBytesParser()
        };
    
        public IReadOnlyList<Record> GetRecords()
        {
            var searcher = new ManagementObjectSearcher("Select * from Win32_DiskDrive")
            {
                Scope = new ManagementScope(@"\root\wmi"),
                Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictData")
            };
            var outputAttributes = new List<Record>();
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
                            outputAttributes.Add(record);
                        }                            
                    }                       
                }
            }
            return outputAttributes;
        }        
    }
}
