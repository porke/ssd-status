namespace SSD_Status.WPF.ViewModels
{
    internal class MainViewModel
    {
        public RawValueInfoViewModel RawValueInfo { get; private set; }
        public DriveInfoViewModel DriveInfo { get; private set; }
        public HistoricalUsageStatsViewModel UsageStatsInfo { get; private set; }
        public RealTimeUsageViewModel RealTimeUsageInfo { get; private set; }

        public MainViewModel()
        {
            RawValueInfo = new RawValueInfoViewModel();
            DriveInfo = new DriveInfoViewModel();
            UsageStatsInfo = new HistoricalUsageStatsViewModel();
            RealTimeUsageInfo = new RealTimeUsageViewModel();
        }
    }
}
