namespace SSD_Status.WPF.ViewModels.Sources
{
    internal enum ChartType
    {
        None,
        CumulativeHostWrittenGbInTime,
        CumulativePowerOnHoursInTime,
        CumulativeHostWrittenGbPerPowerOnHoursInTime,
        CumulativeWearLevellingInTime,
        DistributedHostWrittenGbInTime,
        DistributedHostWrittenGbPerPowerOnHoursInTime,
        DistributedPowerOnHoursInTime
    }
}
