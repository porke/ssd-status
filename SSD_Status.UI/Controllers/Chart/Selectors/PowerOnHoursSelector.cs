using System;
using System.Linq;
using System.Collections.Generic;
using SSD_Status.Core.Model;

namespace SSD_Status.WPF.Controllers.Chart.Selectors
{
    internal class PowerOnHoursSelector : IChartDataSelector
    {
        public string YAxisDescription => "Hours";

        public IEnumerable<KeyValuePair<DateTime, double>> SelectData(IReadOnlyList<SmartDataEntry> entries)
        {
            return entries.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.PowerOnHours));
        }
    }
}
