using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SSD_Status.WPF.ViewModels
{
    internal class UsageStatsInfoViewModel : ViewModelBase
    {
        public ChartViewModel ChartViewModel { get; private set; } = new ChartViewModel();

        private string _sourceDataFile = "SomeFileVeryFarAway.csv";
        private string _selectedChartType = "None";

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

        public ObservableCollection<string> ChartTypes
        {
            get
            {
                return new ObservableCollection<string>
                {
                    "None",
                    "Gigabytes written in time",
                    "Power on hours in time",
                    "Gigabytes written to power on hours in time",
                    "Wear levelling in time"
                };
            }
        }

        public string SelectedType
        {
            get
            {
                return _selectedChartType;
            }
            set
            {
                _selectedChartType = value;
                NotifyPropertyChanged(nameof(SelectedType));                
            }
        }

        public ObservableCollection<string> LifeEstimates { get; } = new ObservableCollection<string>();

        public ICommand OpenFileCommand { get; set; }
        public ICommand LoadChartCommand { get; set; }
    }
}
