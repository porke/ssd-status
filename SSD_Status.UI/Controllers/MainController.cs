using SSD_Status.Core.Api;
using SSD_Status.WPF.ViewModels;
using System;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using SSD_Status.WPF.Utilities;

namespace SSD_Status.WPF.Controllers
{
    internal class MainController
    {
        private MainViewModel _viewModel;

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand LoadRawValuesCommand { get; private set; }

        public MainController(MainViewModel viewModel)
        {
            _viewModel = viewModel;

            OpenFileCommand = new RelayCommand(OpenFileCommand_Execute);
            LoadRawValuesCommand = new RelayCommand(LoadRawValuesCommand_Execute);

            _viewModel.UsageStatsInfo.OpenFileCommand = OpenFileCommand;
            _viewModel.RawValueInfo.RefreshRawValues = LoadRawValuesCommand;            
        }

        private void LoadRawValuesCommand_Execute(object obj)
        {
            _viewModel.RawValueInfo.RawValues.Clear();
            Entry entry = ServiceLocator.SmartEntryReader.ReadAttributes();
            foreach (var record in entry.Records)
            {
                switch (record.Type.Unit)
                {
                    case UnitType.Gigabyte:
                        _viewModel.RawValueInfo.RawValues.Add($"{record.Type.Name} {record.Value.ToString("0.##", CultureInfo.InvariantCulture)} GB");
                        break;
                    case UnitType.Hour:
                        _viewModel.RawValueInfo.RawValues.Add($"{record.Type.Name} {record.Value} {record.Type.Unit}s");
                        break;
                    default:
                        _viewModel.RawValueInfo.RawValues.Add($"{record.Type.Name} {record.Value}");
                        break;
                }
            }            
        }

        private void OpenFileCommand_Execute(object obj)
        {
            using (var openFileDialog = new OpenFileDialog())
            { 
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _viewModel.UsageStatsInfo.SourceDataFile = openFileDialog.FileName;
                    _viewModel.UsageStatsInfo.UsageValues.Clear();

                    var entries = new List<Entry>();
                    foreach (var line in File.ReadAllLines(openFileDialog.FileName).Skip(1))
                    {
                        var fileEntries = line.Split(';');
                        DateTime timestamp = DateTime.ParseExact(fileEntries[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        int powerOnHours = int.Parse(fileEntries[1]);
                        int wearLevelling = int.Parse(fileEntries[2]);
                        double writtenGb = double.Parse(fileEntries[3], CultureInfo.InvariantCulture);

                        var entry = new Entry()
                        {
                            Timestamp = timestamp
                        };
                        entry.Records.Add(new Record { Value = powerOnHours, Type = new RecordType("Power on hours", UnitType.Hour) });
                        entry.Records.Add(new Record { Value = wearLevelling, Type = new RecordType("Wear levelling", UnitType.None) });
                        entry.Records.Add(new Record { Value = writtenGb, Type = new RecordType("Gigabytes written", UnitType.Gigabyte) });
                        entries.Add(entry);
                    }

                    UpdateUsageStatsViewModel(entries);
                }
            }
        }

        private void UpdateUsageStatsViewModel(IReadOnlyList<Entry> entries)
        {
            if (!entries.Any())
            {
                return;
            }

            entries = EntryAggregator.AggregateEntriesByDay(entries);            

            var lastEntry = entries.Last();
            var firstEntry = entries.First();
            double usagePerDay = ServiceLocator.LifeStatsCalculator.CalculateUsagePerDay(firstEntry, lastEntry);
            double hourUsagePerDay = ServiceLocator.LifeStatsCalculator.CalculateHourUsagePerDay(firstEntry, lastEntry);
            double gigabytesPerHour = ServiceLocator.LifeStatsCalculator.CalculateWearPerDay(firstEntry, lastEntry);
            double wearPerDay = ServiceLocator.LifeStatsCalculator.CalculateWearPerDay(firstEntry, lastEntry);
            _viewModel.UsageStatsInfo.LifeEstimates.Clear();
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Usage per day: {usagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Hour usage per day: {hourUsagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} h");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Gigabytes per usage hour: {gigabytesPerHour.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _viewModel.UsageStatsInfo.LifeEstimates.Add($"Wear per day: {wearPerDay.ToString("0.####", CultureInfo.InvariantCulture)}");

            var gigabyteWrittenEntries = entries.Select(x => new KeyValuePair<DateTime, double>(x.Timestamp, x.Records.First(r => r.Type.Unit == UnitType.Gigabyte).Value));            
            foreach (var entry in gigabyteWrittenEntries)
            {
                _viewModel.UsageStatsInfo.UsageValues.Add(entry);
            }
        }
    }
}
