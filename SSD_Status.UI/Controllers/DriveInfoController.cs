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

        private RelayCommand _refreshDriveCommand;

        internal DriveInfoController(DriveInfoViewModel viewModel)
        {
            _viewModel = viewModel;

            _refreshDriveCommand = new RelayCommand(RefreshDriveInfo_Execute);
            _viewModel.RefreshDriveInfoCommand = _refreshDriveCommand;

            LoadDrives();
            _viewModel.RefreshDriveInfoCommand.Execute(null);
        }

        private void LoadDrives()
        {
            _drives = DriveContainer.Drives;

            _viewModel.Drives.Clear();
            foreach (var drive in _drives)
            {
                _viewModel.Drives.Add(drive.Name);
            }
        }

        private void RefreshDriveInfo_Execute(object obj)
        {
            SsdDrive firstDrive = _viewModel.SelectedDrive != string.Empty
                                    ? _drives.FirstOrDefault(x => x.Name == _viewModel.SelectedDrive)
                                    : _drives.FirstOrDefault();

            if (firstDrive != null)
            {
                _viewModel.SelectedDrive = firstDrive.Name;

                _viewModel.DriveInfo.Clear();
                _viewModel.DriveInfo.Add(new GridPropertyViewModel("Serial number", firstDrive.SerialNo));
                _viewModel.DriveInfo.Add(new GridPropertyViewModel("Capacity", $"{firstDrive.CapacityInGb.ToString("#.00", CultureInfo.InvariantCulture)} GB"));
                _viewModel.DriveInfo.Add(new GridPropertyViewModel("Interface type", firstDrive.InterfaceType));
                _viewModel.DriveInfo.Add(new GridPropertyViewModel("Firmware version", firstDrive.FirmwareVersion));
            }
        }
    }
}
