using System;
using System.Collections.Generic;

namespace SSD_Status.Core.Api
{
    public class Entry
    {
        public DateTime Timestamp { get; set; }
        public IList<Record> Records { get; set; }
    }
}
