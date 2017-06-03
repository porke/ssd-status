using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SSD_Status.Core.Api
{
    public class Entry
    {
        public DateTime Timestamp { get; set; }
        public IList<Record> Records { get; set; } = new List<Record>();

        public string Description
        {
            get
            {
                var builder = new StringBuilder("Timestamp;");
                foreach (var value in Records)
                {
                    builder.Append($"{value.Type.Name};");
                }
                return builder.ToString();
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            string dateString = Timestamp.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            builder.Append($"{dateString};");
            foreach (var record in Records)
            {
                builder.Append($"{record.Value};");
            }
            return builder.ToString();
        }
    }
}
