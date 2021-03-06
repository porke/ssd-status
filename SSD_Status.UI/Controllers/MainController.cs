﻿using SSD_Status.WPF.ViewModels;

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;
        private HistoricalUsageStatsController _historicalUsageChartController;
        private RealTimeUsageController _realTimeUsageController;
        private DriveInfoController _driveInfoController;

        public MainController(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            _historicalUsageChartController = new HistoricalUsageStatsController(_viewModel.UsageStatsInfo);
            _realTimeUsageController = new RealTimeUsageController(_viewModel.RealTimeUsageInfo);
            _driveInfoController = new DriveInfoController(_viewModel.DriveInfo);
        }               
    }
}
