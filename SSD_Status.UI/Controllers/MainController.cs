using SSD_Status.WPF.ViewModels;
using System;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using SSD_Status.WPF.Utilities;
using SSD_Status.Core.Model;
using SSD_Status.WPF.Persistence;

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;
        private HistoricalUsageStatsController _historicalUsageChartController;
        private RawValueInfoController _rawValueInfoController;

        private SsdDrive _drive = new SsdDrive();
        private List<SmartDataEntry> _realTimeData = new List<SmartDataEntry>();
        private Timer _realTimeModeTimer = new Timer();        
                
        public RelayCommand ToggleMonitoringCommand { get; private set; }
        public RelayCommand ExportReadingsCommand { get; private set; }

        public MainController(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            _historicalUsageChartController = new HistoricalUsageStatsController(_viewModel.UsageStatsInfo);
            _rawValueInfoController = new RawValueInfoController(_viewModel.RawValueInfo);
                        
            ToggleMonitoringCommand = new RelayCommand(ToggleMonitoringCommand_Execute);
            ExportReadingsCommand = new RelayCommand(ExportReadingsCommand_Execute);
            _viewModel.RealTimeUsageInfo.ToggleMonitoringCommand = ToggleMonitoringCommand;
            _viewModel.RealTimeUsageInfo.ExportReadingsCommand = ExportReadingsCommand;            
        }

        private void ToggleMonitoringCommand_Execute(object obj)
        {
            if (_realTimeModeTimer.Enabled)
            {
                _realTimeModeTimer.Stop();
                _realTimeModeTimer.Tick -= RealTimeModeTimer_Tick;
            }
            else
            {
                _realTimeModeTimer.Interval = 5 * 1000;
                _realTimeModeTimer.Tick += RealTimeModeTimer_Tick;
                _realTimeModeTimer.Start();
            }
        }

        private void RealTimeModeTimer_Tick(object sender, EventArgs e)
        {
            SmartDataEntry smartEntry = _drive.ReadSmartAttributes();
            _realTimeData.Add(smartEntry);
            
            _viewModel.RealTimeUsageInfo.ChartViewModel.SeriesValues.Add(smartEntry.HostWrittenGb);
            _viewModel.RealTimeUsageInfo.ChartViewModel.Timestamps.Add(smartEntry.Timestamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
            _viewModel.RealTimeUsageInfo.ChartViewModel.Minimum = _viewModel.RealTimeUsageInfo.ChartViewModel.SeriesValues.Min();
            _viewModel.RealTimeUsageInfo.ChartViewModel.Maximum = smartEntry.HostWrittenGb;
        }

        private void ExportReadingsCommand_Execute(object obj)
        {

        }        
    }
}
