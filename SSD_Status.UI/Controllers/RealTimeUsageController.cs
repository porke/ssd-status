﻿using System.Linq;
using SSD_Status.Core.Model;
using SSD_Status.WPF.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using SSD_Status.WPF.Persistence;
using System;
using System.Reactive.Linq;
using SSD_Status.WPF.Controllers.Converters;

namespace SSD_Status.WPF.Controllers
{
    internal class RealTimeUsageController
    {
        private RealTimeUsageViewModel _viewModel;

        private SsdDrive _drive = new SsdDrive();
        private List<SmartDataEntry> _realTimeData = new List<SmartDataEntry>();        
        private IDisposable _realTimeSubscription;

        public RelayCommand ToggleMonitoringCommand { get; private set; }
        public RelayCommand ExportReadingsCommand { get; private set; }
        public RelayCommand ToggleStartFromZeroCommand { get; private set; }

        internal RealTimeUsageController(RealTimeUsageViewModel viewModel)
        {
            _viewModel = viewModel;

            ToggleMonitoringCommand = new RelayCommand(ToggleMonitoringCommand_Execute);
            ExportReadingsCommand = new RelayCommand(ExportReadingsCommand_Execute);
            ToggleStartFromZeroCommand = new RelayCommand(ToggleStartFromZero_Execute);
            _viewModel.ToggleMonitoringCommand = ToggleMonitoringCommand;
            _viewModel.ExportReadingsCommand = ExportReadingsCommand;
            _viewModel.ToggleStartFromZero = ToggleStartFromZeroCommand;
        }

        private void ToggleMonitoringCommand_Execute(object obj)
        {
            _viewModel.IsEnabled = !_viewModel.IsEnabled;
            _viewModel.ChartViewModel.SeriesTitle = "Gigabytes written in time";
            if (_viewModel.IsEnabled)
            {
                _viewModel.ChartViewModel.LabelFormatter = x => x.ToString("0.###", CultureInfo.InvariantCulture);
                ReadSmartEntry();

                int seconds = RealTimeIntervalToSecondCountConverter.Convert(_viewModel.SelectedIntervalType.Value);
                _realTimeSubscription = Observable.Interval(TimeSpan.FromSeconds(seconds))
                                                  .ObserveOnDispatcher()
                                                  .Subscribe((x) => ReadSmartEntry(), () => { });                
            }
            else
            {
                _realTimeSubscription.Dispose();
                _realTimeSubscription = null;
            }
        }

        private void ReadSmartEntry()
        {
            SmartDataEntry smartEntry = _drive.ReadSmartAttributes();
            _realTimeData.Add(smartEntry);
            AppendDataEntry();

            double newValue = smartEntry.HostWrittenGb;
            if (_viewModel.StartFromZero)
            {
                newValue -= _realTimeData.First().HostWrittenGb;
            }
            _viewModel.ChartViewModel.SeriesValues.Add(newValue);
            _viewModel.ChartViewModel.Timestamps.Add(smartEntry.Timestamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
            UpdateChartMinMax();            
        }        

        private void AppendDataEntry()
        {
            SmartDataEntry lastEntry = _realTimeData.Last();
            int lastId = _viewModel.DataEntries.LastOrDefault()?.EntryId ?? 0;
            _viewModel.DataEntries.Add(new EntryViewModel
            {
                EntryId = lastId + 1,
                Timestamp = lastEntry.Timestamp,
                HostWrittenGb = lastEntry.HostWrittenGb,
                PowerOnHours = lastEntry.PowerOnHours,
                WearLevelling = lastEntry.WearLevellingCount,
                PercentLifetimeLeft = lastEntry.PercentLifetimeLeft
            });
        }

        private void ExportReadingsCommand_Execute(object obj)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Comma separated values (*.csv)|*.csv";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var exporter = new SmartEntryCsvExporter();
                    exporter.ExportSmartEntries(saveFileDialog.FileName, _realTimeData);
                }
            }
        }

        private void ToggleStartFromZero_Execute(object obj)
        {            
            double minimumValue = _realTimeData.First().HostWrittenGb;
            var hostWriteValues = _realTimeData.Select(x => _viewModel.StartFromZero ? x.HostWrittenGb - minimumValue : x.HostWrittenGb);
            _viewModel.ChartViewModel.SeriesValues.Clear();
            _viewModel.ChartViewModel.SeriesValues.AddRange(hostWriteValues);
            UpdateChartMinMax();
        }

        private void UpdateChartMinMax()
        {
            _viewModel.ChartViewModel.Minimum = _viewModel.ChartViewModel.SeriesValues.Min();
            _viewModel.ChartViewModel.Maximum = _viewModel.ChartViewModel.SeriesValues.Max();
        }
    }
}
