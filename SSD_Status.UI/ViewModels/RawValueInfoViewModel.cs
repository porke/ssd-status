using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SSD_Status.WPF.ViewModels
{
    internal class RawValueInfoViewModel
    {        
        public ObservableCollection<GridPropertyViewModel> RawValues { get; } = new ObservableCollection<GridPropertyViewModel>();
        
        public ICommand RefreshRawValues { get; set; }
    }
}
