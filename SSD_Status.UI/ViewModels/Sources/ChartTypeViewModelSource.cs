using System.Collections.Generic;

namespace SSD_Status.WPF.ViewModels.Sources
{
    internal static class ChartTypeViewModelSource
    {
        public static IEnumerable<ChartTypeViewModel> GetCumulativeChartViewModels()
        {
            yield return new ChartTypeViewModel(ChartType.None, "None");
            yield return new ChartTypeViewModel(ChartType.CumulativeHostWrittenGbInTime, "Gigabytes written in time");
            yield return new ChartTypeViewModel(ChartType.CumulativePowerOnHoursInTime, "Power on hours in time");
            yield return new ChartTypeViewModel(ChartType.CumulativeHostWrittenGbPerPowerOnHoursInTime, "Gigabytes written to power on hours in time");
            yield return new ChartTypeViewModel(ChartType.CumulativeWearLevellingInTime, "Wear levelling in time");            
        }

        public static IEnumerable<ChartTypeViewModel> GetDistributedChartViewModels()
        {
            yield return new ChartTypeViewModel(ChartType.None, "None");
            yield return new ChartTypeViewModel(ChartType.DistributedHostWrittenGbInTime, "Gigabytes written in time");
            yield return new ChartTypeViewModel(ChartType.DistributedHostWrittenGbPerPowerOnHoursInTime, "Gigabytes written to power on hours in time");
            yield return new ChartTypeViewModel(ChartType.DistributedPowerOnHoursInTime, "Power on hours in time");
        }
    }
}
