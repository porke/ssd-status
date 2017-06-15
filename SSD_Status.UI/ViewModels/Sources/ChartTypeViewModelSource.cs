using System.Collections.Generic;

namespace SSD_Status.WPF.ViewModels.Sources
{
    internal static class ChartTypeViewModelSource
    {
        public static IEnumerable<ChartTypeViewModel> GetChartViewModelTypes()
        {
            yield return new ChartTypeViewModel(CumulativeChartType.None, "None");
            yield return new ChartTypeViewModel(CumulativeChartType.HostWrittenGbInTime, "Gigabytes written in time");
            yield return new ChartTypeViewModel(CumulativeChartType.PowerOnHoursInTime, "Power on hours in time");
            yield return new ChartTypeViewModel(CumulativeChartType.HostWrittenGbPerPowerOnHoursInTime, "Gigabytes written to power on hours in time");
            yield return new ChartTypeViewModel(CumulativeChartType.WearLevellingInTime, "Wear levelling in time");
        }
    }
}
