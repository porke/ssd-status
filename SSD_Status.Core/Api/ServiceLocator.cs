using SSD_Status.Core.Implementation;

namespace SSD_Status.Core.Api
{
    public static class ServiceLocator
    {
        public static ISmartReader SmartEntryReader => new SmartReader();

        public static ILifeStatsCalculator LifeStatsCalculator => new LifeStatsCalculator();
    }
}
