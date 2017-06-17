using System;
using System.Collections.Generic;

namespace SSD_Status.WPF.Controllers.Chart
{
    interface IChartDataTransformer
    {
        IEnumerable<KeyValuePair<DateTime, double>> Transform(IEnumerable<KeyValuePair<DateTime, double>> data);
    }
}
