using System;
using System.Collections.Generic;
using System.Linq;
using SSD_Status.Core.Model;

namespace SSD_Status.WPF.Controllers.Chart
{
    internal class WearLevellingSelector : IChartDataSelector
    {
        public string YAxisDescription => "Unit";

        public IEnumerable<KeyValuePair<DateTime, double>> SelectData(IReadOnlyList<SmartDataEntry> entries)
        {
            return entries.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.WearLevellingCount));
        }
    }
}
