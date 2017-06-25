using System.Collections.ObjectModel;

namespace SSD_Status.WPF.ViewModels
{
    internal class DriveInfoViewModel
    {
        public ObservableCollection<string> DriveInfo { get; set; } = new ObservableCollection<string>();
    }
}
