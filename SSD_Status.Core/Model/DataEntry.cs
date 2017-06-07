using System;

namespace SSD_Status.Core.Model
{
    public class SmartDataEntry
    {
        internal SmartDataEntry(DateTime timestamp,
                                double hostWrittenGb,
                                int powerOnHours,
                                int percentLifetimeLeft,
                                int wearLevellingCount,
                                int powerCycleCount)
        {
            Timestamp = timestamp;
            HostWrittenGb = hostWrittenGb;
            PowerOnHours = powerOnHours;
            PercentLifetimeLeft = percentLifetimeLeft;
            WearLevellingCount = wearLevellingCount;
            PowerCycleCount = powerCycleCount;
        }

        public DateTime Timestamp { get; private set; }
        public double HostWrittenGb { get; private set; }
        public int PowerOnHours { get; private set; }
        public int PercentLifetimeLeft { get; private set; }
        public int WearLevellingCount { get; private set; }
        public int PowerCycleCount { get; private set; }
    }
}
