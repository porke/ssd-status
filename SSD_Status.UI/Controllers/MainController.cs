using SSD_Status.WPF.ViewModels;
using System;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using SSD_Status.WPF.Utilities;
using SSD_Status.Core.Model;

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;

        private SsdDrive _drive = new SsdDrive();

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
            _viewModel.RawValueInfo.RawValues.Clear();
            var drive = new SsdDrive();
            var dataEntry = drive.ReadSmartAttributes();
            _viewModel.RawValueInfo.RawValues.Add($"Gb Written: {dataEntry.HostWrittenGb.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _viewModel.RawValueInfo.RawValues.Add($"Power on time: {dataEntry.PowerOnHours} hours");
            _viewModel.RawValueInfo.RawValues.Add($"Percent lifetime: {dataEntry.PercentLifetimeLeft}%");
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

                    var entries = new List<DataEntry>();
                    foreach (var line in File.ReadAllLines(openFileDialog.FileName).Skip(1))
                    {
                        var fileEntries = line.Split(';');
                        DateTime timestamp = DateTime.ParseExact(fileEntries[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        int powerOnHours = int.Parse(fileEntries[1]);
                        int wearLevelling = int.Parse(fileEntries[2]);
                        double writtenGb = double.Parse(fileEntries[3], CultureInfo.InvariantCulture);

                        entries.Add(new DataEntry(timestamp, writtenGb, powerOnHours, 0, wearLevelling));
                    }

                    UpdateUsageStatsViewModel(entries);
                }
            }
        }

        private void LoadChartCommand_Execute(object obj)
        {
            // TODO: implement chart loading
        }

        private void UpdateUsageStatsViewModel(IReadOnlyList<DataEntry> entries)
        {
            if (!entries.Any())
            {
                return;
            }

            entries = EntryAggregator.AggregateEntriesByDay(entries);

            var firstEntry = entries.First();
            var lastEntry = entries.Last();            
            double usagePerDay = _drive.CalculateHostWrittenGbPerDay(firstEntry, lastEntry);
            double hourUsagePerDay = _drive.CalculatePowerOnHoursPerDay(firstEntry, lastEntry);
            double gigabytesPerHour = _drive.CalculateHostWrittenGbPerPowerOnHours(firstEntry, lastEntry);
            double wearPerDay = _drive.CalculateWearLevellingPerDay(firstEntry, lastEntry);
            _viewModel.UsageStatsInfo.LifeEstimates.Clear();
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Usage per day: {usagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Hour usage per day: {hourUsagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} h");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Gigabytes per usage hour: {gigabytesPerHour.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Wear per day: {wearPerDay.ToString("0.####", CultureInfo.InvariantCulture)}");

            var gigabyteWrittenEntries = entries.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.HostWrittenGb));
            foreach (var entry in gigabyteWrittenEntries)
            {
                _viewModel.UsageStatsInfo.ChartViewModel.UsageValues.Add(entry);
            }
        }
    }
}
