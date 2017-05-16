using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SSD_Status.WPF.ViewModels
{
    internal class UsageStatsInfoViewModel : ViewModelBase
    {
        private string _sourceDataFile = "SomeFileVeryFarAway.csv";

        public string SourceDataFile
        {
            get
            {
                return _sourceDataFile;
            }
            set
            {
                _sourceDataFile = value;
                NotifyPropertyChanged(nameof(SourceDataFile));
            }
        }

        public ObservableCollection<string> LifeEstimates
        {
            get
            {
                return new ObservableCollection<string> { "Not", "Very", "Long" };
            }
        }

        public ICommand OpenFileCommand { get; set; }      
    }
}
