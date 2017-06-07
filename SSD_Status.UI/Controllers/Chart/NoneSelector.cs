using System;
using System.Collections.Generic;
using SSD_Status.Core.Model;
using System.Linq;

namespace SSD_Status.WPF.Controllers.Chart
{
    internal class NoneSelector : IChartDataSelector
    {
        public string YAxisDescription => string.Empty;

        public IEnumerable<KeyValuePair<DateTime, double>> SelectData(IReadOnlyList<SmartDataEntry> entries)
        {
            return Enumerable.Empty<KeyValuePair<DateTime, double>>();
        }
    }
}
