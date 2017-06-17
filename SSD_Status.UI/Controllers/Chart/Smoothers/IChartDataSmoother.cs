using System;
using System.Collections.Generic;

namespace SSD_Status.WPF.Controllers.Chart.Smoothers
{
    interface IChartDataSmoother
    {
        IEnumerable<KeyValuePair<DateTime, double>> Smooth(IEnumerable<KeyValuePair<DateTime, double>> data);
    }
}
