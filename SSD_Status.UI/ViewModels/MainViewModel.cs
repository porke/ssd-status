namespace SSD_Status.WPF.ViewModels
{
    internal class MainViewModel
    {
        public DriveInfoViewModel DriveInfo { get; private set; }
        public HistoricalUsageStatsViewModel UsageStatsInfo { get; private set; }
        public RealTimeUsageViewModel RealTimeUsageInfo { get; private set; }

        public MainViewModel()
        {
            DriveInfo = new DriveInfoViewModel();
            UsageStatsInfo = new HistoricalUsageStatsViewModel();
            RealTimeUsageInfo = new RealTimeUsageViewModel();
        }
    }
}
