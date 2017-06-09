using System.Linq;
using SSD_Status.Core.Model;
using SSD_Status.WPF.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Timers;
using System.Windows.Forms;
using SSD_Status.WPF.Persistence;

namespace SSD_Status.WPF.Controllers
{
    internal class RealTimeUsageController
    {
        private RealTimeUsageViewModel _viewModel;

        private SsdDrive _drive = new SsdDrive();
        private List<SmartDataEntry> _realTimeData = new List<SmartDataEntry>();
        private System.Timers.Timer _realTimeModeTimer = new System.Timers.Timer();

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
            if (_realTimeModeTimer.Enabled)
            {
                _realTimeModeTimer.Stop();
                _realTimeModeTimer.Elapsed -= RealTimeModeTimer_Elapsed;
            }
            else
            {
                _realTimeModeTimer.Interval = 5 * 1000;
                _realTimeModeTimer.Elapsed += RealTimeModeTimer_Elapsed;
                _realTimeModeTimer.Start();
            }
        }

        private void RealTimeModeTimer_Elapsed(object sender, ElapsedEventArgs e)
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
