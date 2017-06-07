﻿using System.Linq;
using SSD_Status.Core.Model;
using SSD_Status.WPF.Controllers.Chart;
using SSD_Status.WPF.ViewModels;
using SSD_Status.WPF.ViewModels.Sources;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using SSD_Status.WPF.Utilities;
using SSD_Status.WPF.Persistence;
using System.Windows;

namespace SSD_Status.WPF.Controllers
{
    public class HistoricalUsageStatsController
    {
        private HistoricalUsageStatsViewModel _usageViewModel;

        private SmartEntryCsvImporter _smartEntryCsvImporter = new SmartEntryCsvImporter();

        private Dictionary<ChartType, IChartDataSelector> _dataSelectors = new Dictionary<ChartType, IChartDataSelector>()
            {
                {ChartType.None, new NoneSelector()},
                {ChartType.HostWrittenGbInTime, new HostWritesSelector()},
                {ChartType.HostWrittenGbPerPowerOnHoursInTime, new HostWritesPerHoursOnSelector()},
                {ChartType.WearLevellingInTime, new WearLevellingSelector()},
                {ChartType.PowerOnHoursInTime, new PowerOnHoursSelector()}
            };
        private List<SmartDataEntry> _historicalData = new List<SmartDataEntry>();

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand LoadChartCommand { get; private set; }

        internal HistoricalUsageStatsController(HistoricalUsageStatsViewModel usageViewModel)
        {
            _usageViewModel = usageViewModel;

            LoadChartCommand = new RelayCommand(LoadChartCommand_Execute);
            _usageViewModel.LoadChartCommand = LoadChartCommand;

            OpenFileCommand = new RelayCommand(OpenFileCommand_Execute);
            _usageViewModel.OpenFileCommand = OpenFileCommand;
        }

        private void LoadChartCommand_Execute(object chartType)
        {
            var chartTypeVm = chartType as ChartTypeViewModel;            
            UpdateChart(_historicalData, chartTypeVm);
        }

        private void OpenFileCommand_Execute(object obj)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _usageViewModel.SourceDataFile = openFileDialog.FileName;
                    _historicalData.Clear();

                    _historicalData.AddRange(_smartEntryCsvImporter.ImportSmartEntries(openFileDialog.FileName));
                    CalculateLifeEstimates();
                }
            }
        }

        private void CalculateLifeEstimates()
        {
            _historicalData = EntryAggregator.AggregateEntriesByDay(_historicalData.AsReadOnly()).ToList();

            var firstEntry = _historicalData.First();
            var lastEntry = _historicalData.Last();
            int days = (firstEntry.Timestamp - firstEntry.Timestamp).Days;
            double usagePerDay = (lastEntry.HostWrittenGb - firstEntry.HostWrittenGb) / days;
            double hourUsagePerDay = (lastEntry.PowerOnHours - firstEntry.PowerOnHours) / (double)days;            
            double gigabytesPerHour = (lastEntry.HostWrittenGb - firstEntry.HostWrittenGb) / (lastEntry.PowerOnHours - firstEntry.PowerOnHours);
            double wearPerDay = (lastEntry.WearLevellingCount - firstEntry.WearLevellingCount) / (double)days;

            _usageViewModel.LifeEstimates.Clear();
            _usageViewModel.LifeEstimates.Add($"Usage per day: {usagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _usageViewModel.LifeEstimates.Add($"Hour usage per day: {hourUsagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} h");
            _usageViewModel.LifeEstimates.Add($"Gigabytes per usage hour: {gigabytesPerHour.ToString("0.##", CultureInfo.InvariantCulture)} GB");
            _usageViewModel.LifeEstimates.Add($"Wear per day: {wearPerDay.ToString("0.####", CultureInfo.InvariantCulture)}");
        }

        internal void UpdateChart(IReadOnlyList<SmartDataEntry> records, ChartTypeViewModel chartTypeVm)
        {
            var chartViewModel = _usageViewModel.ChartViewModel;
            chartViewModel.SeriesValues.Clear();
            chartViewModel.Timestamps.Clear();

            IChartDataSelector selector = _dataSelectors[chartTypeVm.Type];
            chartViewModel.YAxisTitle = selector.YAxisDescription;
            chartViewModel.SeriesTitle = chartTypeVm.Description;

            var selectedData = selector.SelectData(records);
            chartViewModel.ChartVisibility = chartTypeVm.Type == ChartType.None ? Visibility.Collapsed : Visibility.Visible;
            chartViewModel.Minimum = selectedData.Any() ? selectedData.Select(x => x.Value).Min() : 0;
            chartViewModel.Maximum = selectedData.Any() ? selectedData.Select(x => x.Value).Max() : 1;
            chartViewModel.Timestamps.AddRange(selectedData.Select(x => x.Key.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            chartViewModel.SeriesValues.AddRange(selectedData.Select(x => x.Value));
        }             
    }
}
