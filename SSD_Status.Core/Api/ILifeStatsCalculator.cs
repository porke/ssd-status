namespace SSD_Status.Core.Api
{
    public interface ILifeStatsCalculator
    {
        double CalculateUsagePerDay(Entry startEntry, Entry endEntry);
    }
}
