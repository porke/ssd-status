using System.Linq;
using SSD_Status.Core.Api;
using System.Collections.Generic;
using System;

namespace SSD_Status.WPF.Utilities
{
    internal static class EntryAggregator
    {
        public static IReadOnlyList<Entry> AggregateEntriesByMonth(IReadOnlyList<Entry> entries)
        {
            var minDate = entries.Select(x => x.Timestamp).Min();
            var maxDate = entries.Select(x => x.Timestamp).Max();
            var currentMonth = new DateTime(minDate.Ticks);
            var months = new List<DateTime>();
            while (currentMonth <= maxDate)
            {
                months.Add(new DateTime(currentMonth.Year, currentMonth.Month, 1));
                currentMonth.AddMonths(1);
            }

            var aggregatedEntries = new List<Entry>();
            DateTime previousMonth = months.First();
            foreach (var month in months)
            {
                var entryAtEndOfMonth = entries.FirstOrDefault(x => x.Timestamp >= month && x.Timestamp < month.AddMonths(1));
                if (entryAtEndOfMonth == null)
                {
                    entryAtEndOfMonth = entries.FirstOrDefault(x => x.Timestamp >= previousMonth && x.Timestamp < previousMonth.AddMonths(1));
                }
                else
                {
                    previousMonth = month;
                }

                aggregatedEntries.Add(entryAtEndOfMonth);
            }

            return aggregatedEntries;
        }

        public static IReadOnlyList<Entry> AggregateEntriesByDay(IReadOnlyList<Entry> entries)
        {
            var minDate = entries.Select(x => x.Timestamp).Min();
            var maxDate = entries.Select(x => x.Timestamp).Max();

            var aggregatedEntries = new List<Entry>();
            var currentDate = new DateTime(minDate.Ticks);
            currentDate.AddDays(1);
            DateTime previousDate = minDate;
            while (currentDate <= maxDate)
            {
                var entryAtDate = entries.FirstOrDefault(x => x.Timestamp == currentDate);                
                if (entryAtDate == null)
                {
                    entryAtDate = entries.FirstOrDefault(x => x.Timestamp == previousDate);
                }
                else
                {
                    previousDate = currentDate;
                }

                aggregatedEntries.Add(entryAtDate);
                currentDate.AddDays(1);
            }

            return aggregatedEntries;
        }
    }
}
