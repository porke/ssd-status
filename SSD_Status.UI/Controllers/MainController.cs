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

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;

        private SsdDrive _drive = new SsdDrive();
        private List<SmartDataEntry> _historicalData = new List<SmartDataEntry>();
        private List<SmartDataEntry> _transformedHistoricalData = new List<SmartDataEntry>();

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand LoadRawValuesCommand { get; private set; }
        public RelayCommand LoadChartCommand { get; private set; }

        public MainController(MainViewModel viewModel)
        {
            _viewModel = viewModel;

            OpenFileCommand = new RelayCommand(OpenFileCommand_Execute);            
            LoadChartCommand = new RelayCommand(LoadChartCommand_Execute);
            _viewModel.UsageStatsInfo.OpenFileCommand = OpenFileCommand;
            _viewModel.UsageStatsInfo.LoadChartCommand = LoadChartCommand;

            LoadRawValuesCommand = new RelayCommand(LoadRawValuesCommand_Execute);
            _viewModel.RawValueInfo.RefreshRawValues = LoadRawValuesCommand;            
        }

        private void LoadRawValuesCommand_Execute(object obj)
        {            
            var drive = new SsdDrive();
            SmartDataEntry dataEntry = drive.ReadSmartAttributes();

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

                    foreach (var line in File.ReadAllLines(openFileDialog.FileName).Skip(1))
                    {
                        var fileEntries = line.Split(';');
                        DateTime timestamp = DateTime.ParseExact(fileEntries[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        int powerOnHours = int.Parse(fileEntries[1]);
                        int wearLevelling = int.Parse(fileEntries[2]);
                        double writtenGb = double.Parse(fileEntries[3], CultureInfo.InvariantCulture);

                        _historicalData.Add(new SmartDataEntry(timestamp, writtenGb, powerOnHours, 0, wearLevelling));
                    }

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
    }
}
