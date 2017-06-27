using System.Collections.ObjectModel;

namespace SSD_Status.WPF.ViewModels
{
    internal class DriveInfoViewModel
    {
        public ObservableCollection<GridPropertyViewModel> DriveInfo { get; set; } = new ObservableCollection<GridPropertyViewModel>();
    }
}
