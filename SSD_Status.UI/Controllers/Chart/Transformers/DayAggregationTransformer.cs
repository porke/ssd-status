using System;
using System.Collections.Generic;
using System.Linq;

namespace SSD_Status.WPF.Controllers.Chart.Transformers
{
    internal class DayAggregationTransformer : IChartDataTransformer
    {
        private int _daysAdvanced;

        public DayAggregationTransformer(int daysAdvanced)
        {
            _daysAdvanced = daysAdvanced;
        }

        public IEnumerable<KeyValuePair<DateTime, double>> Transform(IEnumerable<KeyValuePair<DateTime, double>> data)
        {
            var minDate = data.Select(x => x.Key).Min();
            var maxDate = data.Select(x => x.Key).Max();

            var aggregatedEntries = new List<KeyValuePair<DateTime, double>>();
            var currentDate = new DateTime(minDate.Ticks);
            currentDate.AddDays(_daysAdvanced);
            DateTime previousDate = minDate;
            while (currentDate <= maxDate)
            {
                var entryAtDate = data.FirstOrDefault(x => x.Key.Date == currentDate.Date);
                if (entryAtDate.Equals(default(KeyValuePair<DateTime, double>)))
                {
                    entryAtDate = data.FirstOrDefault(x => x.Key.Date == previousDate.Date);
                }
                else
                {
                    previousDate = currentDate;
                }

                aggregatedEntries.Add(entryAtDate);
                currentDate = currentDate.AddDays(_daysAdvanced);
            }

            return aggregatedEntries;
        }
    }
}
