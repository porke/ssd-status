using System;
using System.Linq;
using System.Collections.Generic;

namespace SSD_Status.WPF.Controllers.Chart.Smoothers
{
    internal class SimpleMovingAverageSmoother : IChartDataSmoother
    {
        private int _period;

        public SimpleMovingAverageSmoother(int period)
        {
            _period = period;
        }

        public IEnumerable<KeyValuePair<DateTime, double>> Smooth(IEnumerable<KeyValuePair<DateTime, double>> data)
        {
            if (data.Count() < _period)
            {
                return data;
            }

            // TODO

            return data;
        }
    }
}
