using System.Linq;
using SSD_Status.Core.Model;
using SSD_Status.WPF.ViewModels;
using System.Collections.Generic;
using System.Globalization;

namespace SSD_Status.WPF.Controllers
{
    internal class DriveInfoController
    {
        private DriveInfoViewModel _viewModel;
        private IReadOnlyList<SsdDrive> _drives = new List<SsdDrive>();
        private SsdDrive _drive;

        private RelayCommand _refreshDriveCommand;
        private RelayCommand _loadRawValuesCommand;

        internal DriveInfoController(DriveInfoViewModel viewModel)
        {
            _viewModel = viewModel;

            _refreshDriveCommand = new RelayCommand(RefreshDriveInfo_Execute);
            _viewModel.RefreshDriveInfoCommand = _refreshDriveCommand;

            _loadRawValuesCommand = new RelayCommand(LoadRawValuesCommand_Execute);
            _viewModel.RefreshRawValues = _loadRawValuesCommand;            

            LoadDrives();
            _refreshDriveCommand.Execute(null);
            _loadRawValuesCommand.Execute(null);
        }

        private void LoadDrives()
        {
            _drives = DriveContainer.Drives;
            _drive = _viewModel.SelectedDrive != string.Empty
                                    ? _drives.FirstOrDefault(x => x.Name == _viewModel.SelectedDrive)
                                    : _drives.FirstOrDefault();

            _viewModel.Drives.Clear();
            foreach (var drive in _drives)
            {
                _viewModel.Drives.Add(drive.Name);
            }
        }

        private void RefreshDriveInfo_Execute(object obj)
        {            
            if (_drive != null)
            {
                _viewModel.SelectedDrive = _drive.Name;

                _viewModel.DriveInfo.Clear();
                _viewModel.DriveInfo.Add(new GridPropertyViewModel("Serial number", _drive.SerialNo));
                _viewModel.DriveInfo.Add(new GridPropertyViewModel("Capacity", $"{_drive.CapacityInGb.ToString("#.00", CultureInfo.InvariantCulture)} GB"));
                _viewModel.DriveInfo.Add(new GridPropertyViewModel("Interface type", _drive.InterfaceType));
                _viewModel.DriveInfo.Add(new GridPropertyViewModel("Firmware version", _drive.FirmwareVersion));
            }
        }        

        private void LoadRawValuesCommand_Execute(object obj)
        {
            SmartDataEntry dataEntry = _drive.ReadSmartAttributes();

            _viewModel.LastRefreshed = dataEntry.Timestamp;
            _viewModel.RawValues.Clear();
            _viewModel.RawValues.Add(new GridPropertyViewModel("Total gigabytes written", $"{dataEntry.HostWrittenGb.ToString("0.##", CultureInfo.InvariantCulture)} GB"));
            _viewModel.RawValues.Add(new GridPropertyViewModel("Power on time", $"{dataEntry.PowerOnHours} hours"));
            _viewModel.RawValues.Add(new GridPropertyViewModel("Lifetime left", $"{dataEntry.PercentLifetimeLeft}%"));
            _viewModel.RawValues.Add(new GridPropertyViewModel("Power cycle count", $"{dataEntry.PowerCycleCount}"));
            _viewModel.RawValues.Add(new GridPropertyViewModel("Wear levelling", $"{dataEntry.WearLevellingCount}"));
        }
    }
}
