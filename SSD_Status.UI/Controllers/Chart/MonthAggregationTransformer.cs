using System;
using System.Linq;
using System.Collections.Generic;

namespace SSD_Status.WPF.Controllers.Chart
{
    internal class MonthAggregationTransformer : IChartDataTransformer
    {
        public IEnumerable<KeyValuePair<DateTime, double>> Transform(IEnumerable<KeyValuePair<DateTime, double>> data)
        {
            var minDate = data.Select(x => x.Key).Min();
            var maxDate = data.Select(x => x.Key).Max();
            var currentMonth = new DateTime(minDate.Ticks);
            var months = new List<DateTime>();
            while (currentMonth <= maxDate)
            {
                months.Add(new DateTime(currentMonth.Year, currentMonth.Month, 1));
                currentMonth = currentMonth.AddMonths(1);
            }

            var aggregatedEntries = new List<KeyValuePair<DateTime, double>>();
            DateTime previousMonth = months.First();
            foreach (var month in months)
            {
                var entryAtEndOfMonth = data.FirstOrDefault(x => x.Key >= month && x.Key < month.AddMonths(1));
                if (entryAtEndOfMonth.Equals(default(KeyValuePair<DateTime, double>)))
                {
                    entryAtEndOfMonth = data.FirstOrDefault(x => x.Key >= previousMonth && x.Key < previousMonth.AddMonths(1));
                }
                else
                {
                    previousMonth = month;
                }

                aggregatedEntries.Add(entryAtEndOfMonth);
            }

            return aggregatedEntries;
        }
    }
}
