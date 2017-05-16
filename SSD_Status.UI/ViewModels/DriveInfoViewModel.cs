using System.Collections.ObjectModel;

namespace SSD_Status.WPF.ViewModels
{
    internal class DriveInfoViewModel : ViewModelBase
    {
        public ObservableCollection<string> DriveInfo
        {
            get
            {
                return new ObservableCollection<string> { "Type", "Stuff", "OtherStuff" };
            }
        }
    }
}
