using System.Linq;
using SSD_Status.Core.Model;
using SSD_Status.WPF.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using SSD_Status.WPF.Persistence;
using System;
using System.Reactive.Linq;

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

        internal RealTimeUsageController(RealTimeUsageViewModel viewModel)
        {
            _viewModel = viewModel;

            ToggleMonitoringCommand = new RelayCommand(ToggleMonitoringCommand_Execute);
            ExportReadingsCommand = new RelayCommand(ExportReadingsCommand_Execute);
            _viewModel.ToggleMonitoringCommand = ToggleMonitoringCommand;
            _viewModel.ExportReadingsCommand = ExportReadingsCommand;
        }

        private void ToggleMonitoringCommand_Execute(object obj)
        {
            if (_realTimeSubscription == null)
            {
                ReadSmartEntry();
                _realTimeSubscription = Observable.Interval(TimeSpan.FromSeconds(5))
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

            _viewModel.ChartViewModel.SeriesValues.Add(smartEntry.HostWrittenGb);
            _viewModel.ChartViewModel.Timestamps.Add(smartEntry.Timestamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
            _viewModel.ChartViewModel.Minimum = _viewModel.ChartViewModel.SeriesValues.Min();
            _viewModel.ChartViewModel.Maximum = smartEntry.HostWrittenGb;
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
    }
}
