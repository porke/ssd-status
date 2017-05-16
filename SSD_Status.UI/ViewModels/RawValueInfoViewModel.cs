using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SSD_Status.WPF.ViewModels
{
    internal class RawValueInfoViewModel : ViewModelBase
    {
        private ObservableCollection<string> _rawValues = new ObservableCollection<string>();

        public ObservableCollection<string> RawValues
        {
            get
            {
                return _rawValues;
            }
            set
            {
                _rawValues = value;
                NotifyPropertyChanged(nameof(RawValues));
            }
        }
    }
}
