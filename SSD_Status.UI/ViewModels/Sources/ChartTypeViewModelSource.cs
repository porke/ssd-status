using SSD_Status.WPF.ViewModels.Enums;
using System.Collections.Generic;

namespace SSD_Status.WPF.ViewModels.Sources
{
    internal static class ChartTypeViewModelSource
    {
        public static IEnumerable<EnumerableViewModel<ChartType>> GetCumulativeChartViewModels()
        {
            yield return new EnumerableViewModel<ChartType>(ChartType.None, "None");
            yield return new EnumerableViewModel<ChartType>(ChartType.CumulativeHostWrittenGbInTime, "Gigabytes written in time");
            yield return new EnumerableViewModel<ChartType>(ChartType.CumulativePowerOnHoursInTime, "Power on hours in time");
            yield return new EnumerableViewModel<ChartType>(ChartType.CumulativeHostWrittenGbPerPowerOnHoursInTime, "Gigabytes written to power on hours in time");
            yield return new EnumerableViewModel<ChartType>(ChartType.CumulativeWearLevellingInTime, "Wear levelling in time");            
        }

        public static IEnumerable<EnumerableViewModel<ChartType>> GetDistributedChartViewModels()
        {
            yield return new EnumerableViewModel<ChartType>(ChartType.None, "None");
            yield return new EnumerableViewModel<ChartType>(ChartType.DistributedHostWrittenGbInTime, "Gigabytes written in time");
            yield return new EnumerableViewModel<ChartType>(ChartType.DistributedPowerOnHoursInTime, "Power on hours in time");
        }
    }
}
