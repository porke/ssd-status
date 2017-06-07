using SSD_Status.WPF.ViewModels;

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;
        private HistoricalUsageStatsController _historicalUsageChartController;
        private RealTimeUsageController _realTimeUsageController;
        private RawValueInfoController _rawValueInfoController;        

        public MainController(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            _historicalUsageChartController = new HistoricalUsageStatsController(_viewModel.UsageStatsInfo);
            _rawValueInfoController = new RawValueInfoController(_viewModel.RawValueInfo);
            _realTimeUsageController = new RealTimeUsageController(_viewModel.RealTimeUsageInfo);
        }               
    }
}
