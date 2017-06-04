using System;
using System.Linq;
using SSD_Status.Core.Api;

namespace SSD_Status.Core.Implementation
{
    internal class LifeStatsCalculator : ILifeStatsCalculator
    {
        public double CalculateGigabytesPerHour(Entry startEntry, Entry endEntry)
        {
            int startHours = (int)startEntry.Records.First(x => x.Type.Unit == UnitType.Hour).Value;
            int endHours = (int)endEntry.Records.First(x => x.Type.Unit == UnitType.Hour).Value;
            double startValue = startEntry.Records.First(x => x.Type.Unit == UnitType.Gigabyte).Value;
            double endValue = endEntry.Records.First(x => x.Type.Unit == UnitType.Gigabyte).Value;
            return (endValue - startValue) / (endHours - startHours);
        }

        public double CalculateHourUsagePerDay(Entry startEntry, Entry endEntry)
        {
            int days = (endEntry.Timestamp - startEntry.Timestamp).Days;
            double startValue = startEntry.Records.First(x => x.Type.Unit == UnitType.Hour).Value;
            double endValue = endEntry.Records.First(x => x.Type.Unit == UnitType.Hour).Value;
            return (endValue - startValue) / days;
        }

        public double CalculateUsagePerDay(Entry startEntry, Entry endEntry)
        {
            int days = (endEntry.Timestamp - startEntry.Timestamp).Days;
            double startValue = startEntry.Records.First(x => x.Type.Unit == UnitType.Gigabyte).Value;
            double endValue = endEntry.Records.First(x => x.Type.Unit == UnitType.Gigabyte).Value;
            return (endValue - startValue) / days;
        }

        public double CalculateWearPerDay(Entry startEntry, Entry endEntry)
        {
            int days = (endEntry.Timestamp - startEntry.Timestamp).Days;
            double startValue = startEntry.Records.First(x => x.Type.Unit == UnitType.None).Value;
            double endValue = endEntry.Records.First(x => x.Type.Unit == UnitType.None).Value;
            return (endValue - startValue) / days;
        }
    }
}
