using System.Linq;
using SSD_Status.Core.Api;

namespace SSD_Status.Core.Implementation
{
    internal class LifeStatsCalculator : ILifeStatsCalculator
    {
        public double CalculateUsagePerDay(Entry startEntry, Entry endEntry)
        {
            int days = (endEntry.Timestamp - startEntry.Timestamp).Days;
            double startValue = startEntry.Records.First(x => x.Type.Unit == UnitType.Gigabyte).Value;
            double endValue = endEntry.Records.First(x => x.Type.Unit == UnitType.Gigabyte).Value;
            return (endValue - startValue) / days;
        }
    }
}
