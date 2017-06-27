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

        public DriveInfoController(DriveInfoViewModel viewModel)
        {
            _viewModel = viewModel;

            LoadDriveInfo();
        }

        private void LoadDriveInfo()
        {
            var first = DriveContainer.Drives[0];
            _viewModel.DriveInfo.Add(new GridPropertyViewModel("Model", first.Name));
            _viewModel.DriveInfo.Add(new GridPropertyViewModel("Serial number", first.SerialNo));
            _viewModel.DriveInfo.Add(new GridPropertyViewModel("Capacity", $"{first.CapacityInGb.ToString("#.00", CultureInfo.InvariantCulture)} GB"));
            _viewModel.DriveInfo.Add(new GridPropertyViewModel("Interface type", first.InterfaceType));
            _viewModel.DriveInfo.Add(new GridPropertyViewModel("Firmware version", first.FirmwareVersion));
        }
    }
}
