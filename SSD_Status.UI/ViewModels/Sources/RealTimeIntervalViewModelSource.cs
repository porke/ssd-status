using SSD_Status.WPF.ViewModels.Enums;
using System.Collections.Generic;

namespace SSD_Status.WPF.ViewModels.Sources
{
    internal static class RealTimeIntervalViewModelSource
    {
        public static IEnumerable<EnumerableViewModel<RealTimeIntervalType>> GetRealTimeIntervalTypes()
        {
            yield return new EnumerableViewModel<RealTimeIntervalType>(RealTimeIntervalType.Seconds_15, "15 seconds");
            yield return new EnumerableViewModel<RealTimeIntervalType>(RealTimeIntervalType.Minutes_1, "1 minute");
            yield return new EnumerableViewModel<RealTimeIntervalType>(RealTimeIntervalType.Minutes_5, "5 minutes");
            yield return new EnumerableViewModel<RealTimeIntervalType>(RealTimeIntervalType.Minutes_15, "15 minutes");
            yield return new EnumerableViewModel<RealTimeIntervalType>(RealTimeIntervalType.Hours_1, "1 hour");
            yield return new EnumerableViewModel<RealTimeIntervalType>(RealTimeIntervalType.Hours_4, "4 hours");
        }
    }
}
