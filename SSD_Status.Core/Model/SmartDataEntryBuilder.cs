using System;

namespace SSD_Status.Core.Model
{
    public class SmartDataEntryBuilder
    {
        public DateTime Timestamp { private get; set; } = DateTime.Now;
        public double HostWrittenGb { private get; set; }
        public int PowerOnHours { private get; set; }
        public int PercentLifetimeLeft { private get; set; }
        public int WearLevellingCount { private get; set; }
        public int PowerCycleCount { private get; set; }

        public SmartDataEntry Build()
        {
            return new SmartDataEntry(Timestamp,
                                      HostWrittenGb,
                                      PowerOnHours,
                                      PercentLifetimeLeft,
                                      WearLevellingCount,
                                      PowerCycleCount);
        }
    }
}
