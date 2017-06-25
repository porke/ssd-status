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
            if (data.Count() < _period || _period <= 1)
            {
                return data;
            }

            var returnList = new List<KeyValuePair<DateTime, double>>();
            for (int i = _period; i < data.Count(); ++i)
            {
                double average = data.Skip(i).Take(_period).Select(x => x.Value).Average();
                returnList.Add(new KeyValuePair<DateTime, double>(data.Skip(i).First().Key, average));
            }

            return returnList;
        }
    }
}
