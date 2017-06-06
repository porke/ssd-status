using System.Collections.Generic;

namespace SSD_Status.WPF.ViewModels.Sources
{
    internal static class ChartTypeViewModelSource
    {
        public static IEnumerable<ChartTypeViewModel> GetChartViewModelTypes()
        {
            yield return new ChartTypeViewModel(ChartType.None, "None");
            yield return new ChartTypeViewModel(ChartType.HostWrittenGbInTime, "Gigabytes written in time");
            yield return new ChartTypeViewModel(ChartType.PowerOnHoursInTime, "Power on hours in time");
            yield return new ChartTypeViewModel(ChartType.HostWrittenGbPerPowerOnHoursInTime, "Gigabytes written to power on hours in time");
            yield return new ChartTypeViewModel(ChartType.WearLevellingInTime, "Wear levelling in time");
        }
    }
}
