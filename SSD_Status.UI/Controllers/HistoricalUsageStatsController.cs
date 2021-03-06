﻿using System.Linq;
using SSD_Status.Core.Model;
using SSD_Status.WPF.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using SSD_Status.WPF.Persistence;
using System.Windows;
using SSD_Status.WPF.Controllers.Chart.Selectors;
using SSD_Status.WPF.Controllers.Chart.Transformers;
using SSD_Status.WPF.ViewModels.Enums;
using System;
using SSD_Status.WPF.Controllers.Chart.Smoothers;

namespace SSD_Status.WPF.Controllers
{
    public class HistoricalUsageStatsController
    {
        private HistoricalUsageStatsViewModel _usageViewModel;
        private ChartViewModel _chartViewModel;        

        private Dictionary<ChartType, IChartDataSelector> _dataSelectors = new Dictionary<ChartType, IChartDataSelector>()
          {
            { ChartType.None, new NoneSelector() },
            { ChartType.CumulativeHostWrittenGbInTime, new HostWritesSelector() },
            { ChartType.CumulativeHostWrittenGbPerPowerOnHoursInTime, new HostWritesPerHoursOnSelector() },
            { ChartType.CumulativeWearLevellingInTime, new WearLevellingSelector() },
            { ChartType.CumulativePowerOnHoursInTime, new PowerOnHoursSelector() }
        };
        private Dictionary<AggregationType, IChartDataTransformer> _dataTransformers = new Dictionary<AggregationType, IChartDataTransformer>()
        {
            { AggregationType.None, new IdentityDataTransformer()},
            { AggregationType.Day, new DayAggregationTransformer(1)},
            { AggregationType.Week, new DayAggregationTransformer(7)},
            { AggregationType.Fortnight, new DayAggregationTransformer(14)},
            { AggregationType.Month, new MonthAggregationTransformer()}
        };
        private List<SmartDataEntry> _historicalData = new List<SmartDataEntry>();

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand RefreshChartCommand { get; private set; }

        internal HistoricalUsageStatsController(HistoricalUsageStatsViewModel usageViewModel)
        {
            _usageViewModel = usageViewModel;
            _chartViewModel = _usageViewModel.ChartViewModel;

            RefreshChartCommand = new RelayCommand(RefreshChartCommand_Execute);
            _usageViewModel.RefreshChartCommand = RefreshChartCommand;

            OpenFileCommand = new RelayCommand(OpenFileCommand_Execute);
            _usageViewModel.OpenFileCommand = OpenFileCommand;
        }

        private void RefreshChartCommand_Execute(object chartType)
        {
            _chartViewModel.SeriesValues.Clear();
            _chartViewModel.Timestamps.Clear();

            IChartDataSelector selector = _dataSelectors[_usageViewModel.SelectedChartType.Value];
            _chartViewModel.SeriesTitle = _usageViewModel.SelectedChartType.Description;
            _chartViewModel.YAxisTitle = selector.YAxisDescription;
            IEnumerable<KeyValuePair<DateTime, double>> chartableData = selector.SelectData(_historicalData);

            if (chartableData.Any())
            {
                IChartDataTransformer transformer;
                IChartDataSmoother smoother;                
                if (_usageViewModel.ChartCategory == ChartCategory.Cumulative)
                {
                    transformer = _dataTransformers[_usageViewModel.SelectedAggregationType.Value];
                    smoother = new SimpleMovingAverageSmoother(1);
                }
                else
                {
                    transformer = new DifferentialDataTransformer();
                    smoother = new SimpleMovingAverageSmoother(_usageViewModel.MovingAveragePeriod);
                }

                chartableData = transformer.Transform(chartableData);
                chartableData = smoother.Smooth(chartableData);
            }

            _chartViewModel.ChartVisibility = _usageViewModel.SelectedChartType.Value == ChartType.None ? Visibility.Collapsed : Visibility.Visible;
            _chartViewModel.Minimum = chartableData.Any() ? chartableData.Select(x => x.Value).Min() : 0;
            _chartViewModel.Maximum = chartableData.Any() ? chartableData.Select(x => x.Value).Max() : 1;
            _chartViewModel.Timestamps.AddRange(chartableData.Select(x => x.Key.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            _chartViewModel.LabelFormatter = x => x.ToString("0.##", CultureInfo.InvariantCulture);
            _chartViewModel.SeriesValues.AddRange(chartableData.Select(x => x.Value));
        }

        private void OpenFileCommand_Execute(object obj)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _usageViewModel.SourceDataFile = openFileDialog.FileName;
                    _historicalData.Clear();

                    var importer = new SmartEntryCsvImporter();
                    _historicalData.AddRange(importer.ImportSmartEntries(openFileDialog.FileName));
                    CalculateLifeEstimates();
                }
            }
        }

        private void CalculateLifeEstimates()
        {            
            var firstEntry = _historicalData.First();
            var lastEntry = _historicalData.Last();
            int days = (lastEntry.Timestamp - firstEntry.Timestamp).Days;
            double usagePerDay = (lastEntry.HostWrittenGb - firstEntry.HostWrittenGb) / days;
            double hourUsagePerDay = (lastEntry.PowerOnHours - firstEntry.PowerOnHours) / (double)days;            
            double gigabytesPerHour = (lastEntry.HostWrittenGb - firstEntry.HostWrittenGb) / (lastEntry.PowerOnHours - firstEntry.PowerOnHours);
            double wearPerDay = (lastEntry.WearLevellingCount - firstEntry.WearLevellingCount) / (double)days;

            _usageViewModel.LifeEstimates.Clear();
            _usageViewModel.LifeEstimates.Add(new GridPropertyViewModel("Total days recorded", days.ToString()));
            _usageViewModel.LifeEstimates.Add(new GridPropertyViewModel("Usage per day", $"{usagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} GB"));
            _usageViewModel.LifeEstimates.Add(new GridPropertyViewModel("Hour usage per day", $"{hourUsagePerDay.ToString("0.##", CultureInfo.InvariantCulture)} h"));
            _usageViewModel.LifeEstimates.Add(new GridPropertyViewModel("Gigabytes per usage hour", $"{gigabytesPerHour.ToString("0.##", CultureInfo.InvariantCulture)} GB"));
            _usageViewModel.LifeEstimates.Add(new GridPropertyViewModel("Wear per day", $"{wearPerDay.ToString("0.####", CultureInfo.InvariantCulture)}"));
        }             
    }
}
