using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SSD_Status.WPF.ViewModels
{
    internal class RawValueInfoViewModel : ViewModelBase
    {        
        public ObservableCollection<string> RawValues { get; } = new ObservableCollection<string>();
        
        public ICommand RefreshRawValues { get; set; }
    }
}
