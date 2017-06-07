using System.Windows.Input;

namespace SSD_Status.WPF.ViewModels
{
    internal class RealTimeUsageViewModel : ViewModelBase
    {
        public ChartViewModel ChartViewModel { get; } = new ChartViewModel();

        public ICommand ExportReadingsCommand { get; set; }
        public ICommand ToggleMonitoringCommand { get; set; }
    }
}
