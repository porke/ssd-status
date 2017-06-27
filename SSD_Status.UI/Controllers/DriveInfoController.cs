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
            _viewModel.DriveInfo.Add(new GridPropertyViewModel { Name = "Model", Value = first.Name });
            _viewModel.DriveInfo.Add(new GridPropertyViewModel { Name = "Serial number", Value = first.SerialNo });
            _viewModel.DriveInfo.Add(new GridPropertyViewModel { Name = "Capacity", Value = $"{first.CapacityInGb.ToString("#.00", CultureInfo.InvariantCulture)} GB" });
        }
    }
}
