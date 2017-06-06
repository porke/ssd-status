using System;

namespace SSD_Status.Core.Model
{
    public class DataEntry
    {
        public DataEntry(DateTime timestamp, double hostWrittenGb, int powerOnHours, int percentLifetimeLeft, int wearLevellingCount)
        {
            Timestamp = timestamp;
            HostWrittenGb = hostWrittenGb;
            PowerOnHours = powerOnHours;
            PercentLifetimeLeft = percentLifetimeLeft;
            WearLevellingCount = wearLevellingCount;
        }

        public DateTime Timestamp { get; private set; }
        public double HostWrittenGb { get; private set; }
        public int PowerOnHours { get; private set; }
        public int PercentLifetimeLeft { get; private set; }
        public int WearLevellingCount { get; private set; }
    }
}
