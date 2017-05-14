using System;

namespace SSD_Status.Core.Api
{
    public class Record
    {
        public DateTime Timestamp { get; set; }
        public decimal Value { get; set; }
        public RecordType Type {get; set;}
    }
}
