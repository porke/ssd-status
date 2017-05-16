using System.Collections.ObjectModel;

namespace SSD_Status.WPF.ViewModels
{
    internal class RawValueInfoViewModel
    {
        public ObservableCollection<string> RawValues
        {
            get
            {
                return new ObservableCollection<string> { "One", "Two", "Three" };
            }
        }
    }
}
