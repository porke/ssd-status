using System;
using System.Collections.Generic;

namespace SSD_Status.WPF.Controllers.Chart
{
    internal class IdentityDataTransformer : IChartDataTransformer
    {
        public IEnumerable<KeyValuePair<DateTime, double>> Transform(IEnumerable<KeyValuePair<DateTime, double>> data)
        {
            return data;
        }
    }
}
