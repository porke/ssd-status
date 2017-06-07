using System;
using System.Linq;
using System.Collections.Generic;
using SSD_Status.Core.Model;

namespace SSD_Status.WPF.Controllers.Chart
{
    internal class HostWritesPerHoursOnSelector : IChartDataSelector
    {
        public string YAxisDescription => "Gigabytes per Hour";

        public IEnumerable<KeyValuePair<DateTime, double>> SelectData(IReadOnlyList<SmartDataEntry> entries)
        {
            return entries.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.HostWrittenGb / x.PowerOnHours));
        }
    }
}
