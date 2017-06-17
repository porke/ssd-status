using System.Linq;
using System.Collections.Generic;
using System;
using SSD_Status.Core.Model;

namespace SSD_Status.WPF.Utilities
{
    internal static class EntryAggregator
    {
        public static IReadOnlyList<SmartDataEntry> AggregateEntriesByDay(IReadOnlyList<SmartDataEntry> entries)
        {
            var minDate = entries.Select(x => x.Timestamp).Min();
            var maxDate = entries.Select(x => x.Timestamp).Max();

            var aggregatedEntries = new List<SmartDataEntry>();
            var currentDate = new DateTime(minDate.Ticks);
            currentDate.AddDays(1);
            DateTime previousDate = minDate;
            while (currentDate <= maxDate)
            {
                var entryAtDate = entries.FirstOrDefault(x => x.Timestamp.Date == currentDate.Date);                
                if (entryAtDate == null)
                {
                    entryAtDate = entries.FirstOrDefault(x => x.Timestamp.Date == previousDate.Date);
                }
                else
                {
                    previousDate = currentDate;
                }

                aggregatedEntries.Add(entryAtDate);
                currentDate = currentDate.AddDays(1);
            }

            return aggregatedEntries;
        }
    }
}
