namespace SSD_Status.Core.Api
{
    public interface ILifeStatsCalculator
    {
        double CalculateUsagePerDay(Entry startEntry, Entry endEntry);
        double CalculateHourUsagePerDay(Entry startEntry, Entry endEntry);
        double CalculateGigabytesPerHour(Entry startEntry, Entry endEntry);
        double CalculateWearPerDay(Entry startEntry, Entry endEntry);
    }
}
