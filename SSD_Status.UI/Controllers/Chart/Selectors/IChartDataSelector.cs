using SSD_Status.Core.Model;
using System;
using System.Collections.Generic;

namespace SSD_Status.WPF.Controllers.Chart.Selectors
{
    interface IChartDataSelector
    {
        IEnumerable<KeyValuePair<DateTime, double>> SelectData(IReadOnlyList<SmartDataEntry> entries);
        string YAxisDescription { get; }
    }
}
