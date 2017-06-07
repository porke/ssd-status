using SSD_Status.WPF.ViewModels;
using System;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using SSD_Status.WPF.Utilities;
using SSD_Status.Core.Model;
using SSD_Status.WPF.ViewModels.Sources;
using SSD_Status.WPF.Persistence;

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;
        
        private SsdDrive _drive = new SsdDrive();
        private List<SmartDataEntry> _historicalData = new List<SmartDataEntry>();
        private List<SmartDataEntry> _transformedHistoricalData = new List<SmartDataEntry>();
        private List<SmartDataEntry> _realTimeData = new List<SmartDataEntry>();
        private Timer _readTimeModeTimer = new Timer();
        private SmartEntryCsvImporter _smartEntryCsvImporter = new SmartEntryCsvImporter();

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand LoadRawValuesCommand { get; private set; }
        public RelayCommand LoadChartCommand { get; private set; }
        public RelayCommand ToggleMonitoringCommand { get; private set; }
        public RelayCommand ExportReadingsCommand { get; private set; }

        public MainController(MainViewModel viewModel)
        {
            _viewModel = viewModel;

            OpenFileCommand = new RelayCommand(OpenFileCommand_Execute);            
            LoadChartCommand = new RelayCommand(LoadChartCommand_Execute);
            _viewModel.UsageStatsInfo.OpenFileCommand = OpenFileCommand;
            _viewModel.UsageStatsInfo.LoadChartCommand = LoadChartCommand;

            ToggleMonitoringCommand = new RelayCommand(ToggleMonitoringCommand_Execute);
            ExportReadingsCommand = new RelayCommand(ExportReadingsCommand_Execute);
            _viewModel.RealTimeUsageInfo.ToggleMonitoringCommand = ToggleMonitoringCommand;
            _viewModel.RealTimeUsageInfo.ExportReadingsCommand = ExportReadingsCommand;

            LoadRawValuesCommand = new RelayCommand(LoadRawValuesCommand_Execute);
            _viewModel.RawValueInfo.RefreshRawValues = LoadRawValuesCommand;            
        }

        private void LoadRawValuesCommand_Execute(object obj)
        {            
            SmartDataEntry dataEntry = _drive.ReadSmartAttributes();

            _viewModel.RawValueInfo.RawValues.Clear();
            _viewModel.RawValueInfo.RawValues.Add($"Gb Written: {dataEntry.HostWrittenGb.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _viewModel.RawValueInfo.RawValues.Add($"Power on time: {dataEntry.PowerOnHours} hours");
            _viewModel.RawValueInfo.RawValues.Add($"Percent lifetime left: {dataEntry.PercentLifetimeLeft}%");
            _viewModel.RawValueInfo.RawValues.Add($"Wear levelling: {dataEntry.WearLevellingCount}");
        }

        private void OpenFileCommand_Execute(object obj)
        {
            using (var openFileDialog = new OpenFileDialog())
            { 
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _viewModel.UsageStatsInfo.SourceDataFile = openFileDialog.FileName;
                    _viewModel.UsageStatsInfo.ChartViewModel.UsageValues.Clear();
                    _historicalData.Clear();

                    _historicalData.AddRange(_smartEntryCsvImporter.ImportSmartEntries(openFileDialog.FileName));                    
                    UpdateUsageStatsViewModel();
                }
            }
        }

        private void LoadChartCommand_Execute(object chartType)
        {
            var chartTypeVm = chartType as ChartTypeViewModel;
            IEnumerable<KeyValuePair<DateTime, double>> records = new List<KeyValuePair<DateTime, double>>();
            switch (chartTypeVm.Type)
            {                
                case ChartType.PowerOnHoursInTime:                    
                    records = _transformedHistoricalData.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.PowerOnHours));
                    _viewModel.UsageStatsInfo.ChartViewModel.YAxisTitle = "Hours";
                    break;
                case ChartType.WearLevellingInTime:
                    records = _transformedHistoricalData.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.WearLevellingCount));
                    _viewModel.UsageStatsInfo.ChartViewModel.YAxisTitle = "Unit";
                    break;
                case ChartType.HostWrittenGbInTime:
                    records = _transformedHistoricalData.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.HostWrittenGb));
                    _viewModel.UsageStatsInfo.ChartViewModel.YAxisTitle = "Gigabytes";
                    break;
                case ChartType.HostWrittenGbPerPowerOnHoursInTime:
                    records = _transformedHistoricalData.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.HostWrittenGb / x.PowerOnHours));
                    _viewModel.UsageStatsInfo.ChartViewModel.YAxisTitle = "Gigabytes per Hour";                    
                    break;
            }

            _viewModel.UsageStatsInfo.ChartViewModel.Title = chartTypeVm.Description;
            _viewModel.UsageStatsInfo.ChartViewModel.UsageValues.Clear();
            _viewModel.UsageStatsInfo.ChartViewModel.UsageValues.AddRange(records);
        }

        private void UpdateUsageStatsViewModel()
        {
            _transformedHistoricalData = EntryAggregator.AggregateEntriesByDay(_historicalData.AsReadOnly()).ToList();

            var firstEntry = _transformedHistoricalData.First();
            var lastEntry = _transformedHistoricalData.Last();            
            double usagePerDay = _drive.CalculateHostWrittenGbPerDay(firstEntry, lastEntry);
            double hourUsagePerDay = _drive.CalculatePowerOnHoursPerDay(firstEntry, lastEntry);
            double gigabytesPerHour = _drive.CalculateHostWrittenGbPerPowerOnHours(firstEntry, lastEntry);
            double wearPerDay = _drive.CalculateWearLevellingPerDay(firstEntry, lastEntry);

            _viewModel.UsageStatsInfo.LifeEstimates.Clear();
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Usage per day: {usagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Hour usage per day: {hourUsagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} h");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Gigabytes per usage hour: {gigabytesPerHour.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Wear per day: {wearPerDay.ToString("0.####", CultureInfo.InvariantCulture)}");            
        }

        private void ToggleMonitoringCommand_Execute(object obj)
        {
            if (_readTimeModeTimer.Enabled)
            {
                _readTimeModeTimer.Stop();
                _readTimeModeTimer.Tick -= _readTimeModeTimer_Tick;
            }
            else
            {
                _readTimeModeTimer.Interval = 5 * 1000;
                _readTimeModeTimer.Tick += _readTimeModeTimer_Tick;
                _readTimeModeTimer.Start();
            }
        }

        private void _readTimeModeTimer_Tick(object sender, EventArgs e)
        {
            SmartDataEntry smartEntry = _drive.ReadSmartAttributes();
            _realTimeData.Add(smartEntry);
            _viewModel.RealTimeUsageInfo.ChartViewModel.UsageValues.Add(new KeyValuePair<DateTime, double>(smartEntry.Timestamp, smartEntry.HostWrittenGb));
        }

        private void ExportReadingsCommand_Execute(object obj)
        {

        }        
    }
}
