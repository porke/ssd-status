using System;
using System.Collections.Generic;
using System.Linq;
using SSD_Status.Core.Model;

namespace SSD_Status.WPF.Controllers.Chart
{
    internal class HostWritesSelector : IChartDataSelector
    {
        public string YAxisDescription => "Gigabytes";

        public IEnumerable<KeyValuePair<DateTime, double>> SelectData(IReadOnlyList<SmartDataEntry> entries)
        {
            return entries.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.HostWrittenGb));
        }
    }
}
