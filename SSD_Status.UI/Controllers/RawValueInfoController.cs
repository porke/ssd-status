using SSD_Status.Core.Model;
using SSD_Status.WPF.ViewModels;
using System.Globalization;

namespace SSD_Status.WPF.Controllers
{
    internal class RawValueInfoController
    {
        private RawValueInfoViewModel _viewModel;
        private SsdDrive _drive = new SsdDrive();

        public RelayCommand LoadRawValuesCommand { get; private set; }

        internal RawValueInfoController(RawValueInfoViewModel viewModel)
        {
            _viewModel = viewModel;

            LoadRawValuesCommand = new RelayCommand(LoadRawValuesCommand_Execute);
            _viewModel.RefreshRawValues = LoadRawValuesCommand;

            LoadRawValuesCommand.Execute(null);
        }

        private void LoadRawValuesCommand_Execute(object obj)
        {
            SmartDataEntry dataEntry = _drive.ReadSmartAttributes();

            _viewModel.LastRefreshed = dataEntry.Timestamp;
            _viewModel.RawValues.Clear();
            _viewModel.RawValues.Add(new GridPropertyViewModel("Total gigabytes written", $"{dataEntry.HostWrittenGb.ToString("0.##", CultureInfo.InvariantCulture)} GB"));
            _viewModel.RawValues.Add(new GridPropertyViewModel("Power on time", $"{dataEntry.PowerOnHours} hours" ));
            _viewModel.RawValues.Add(new GridPropertyViewModel("Lifetime left", $"{dataEntry.PercentLifetimeLeft}%" ));
            _viewModel.RawValues.Add(new GridPropertyViewModel("Power cycle count", $"{dataEntry.PowerCycleCount}" ));
            _viewModel.RawValues.Add(new GridPropertyViewModel("Wear levelling", $"{dataEntry.WearLevellingCount}" ));            
        }
    }
}
