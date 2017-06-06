using System.Linq;
using SSD_Status.WPF.ViewModels.Sources;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SSD_Status.WPF.ViewModels
{
    internal class UsageStatsInfoViewModel : ViewModelBase
    {
        public ChartViewModel ChartViewModel { get; private set; } = new ChartViewModel();

        private string _sourceDataFile = "SomeFileVeryFarAway.csv";
        private ChartTypeViewModel _selectedChartType = ChartTypeViewModelSource.GetChartViewModelTypes().First();

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
                return new ObservableCollection<string>(ChartTypeViewModelSource.GetChartViewModelTypes().Select(x => x.Description));
            }
        }

        public ChartTypeViewModel SelectedChartType
        {
            get
            {
                return _selectedChartType;
            }
            set
            {
                _selectedChartType = value;
                NotifyPropertyChanged(nameof(SelectedChartType));                
            }
        }

        public ObservableCollection<string> LifeEstimates { get; } = new ObservableCollection<string>();

        public ICommand OpenFileCommand { get; set; }
        public ICommand LoadChartCommand { get; set; }
    }
}
