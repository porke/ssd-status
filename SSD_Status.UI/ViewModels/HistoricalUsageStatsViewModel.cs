using System.Linq;
using SSD_Status.WPF.ViewModels.Sources;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using System.Reactive.Linq;

namespace SSD_Status.WPF.ViewModels
{
    internal class HistoricalUsageStatsViewModel : ReactiveObject
    {
        private string _sourceDataFile = "SomeFileVeryFarAway.csv";
        private ChartTypeViewModel _selectedChartType = ChartTypeViewModelSource.GetCumulativeChartViewModels().First();
        private ChartCategory _chartCategory = ChartCategory.Cumulative;

        private ObservableAsPropertyHelper<bool> _isCumulativeChartCategoryActive;
        private ObservableAsPropertyHelper<bool> _isDistributedChartCategoryActive;
        private ObservableAsPropertyHelper<ObservableCollection<string>> _chartTypes;

        public ChartViewModel ChartViewModel { get; } = new ChartViewModel();

        public HistoricalUsageStatsViewModel()
        {
            _isCumulativeChartCategoryActive = this.ObservableForProperty(vm => vm.ChartCategory, skipInitial: false)
                                                   .Select(prop => prop.Value == ChartCategory.Cumulative)
                                                   .ToProperty(this, vm => vm.IsCumulativeChartCategoryActive);

            _isDistributedChartCategoryActive = this.ObservableForProperty(vm => vm.ChartCategory, skipInitial: false)
                                                   .Select(prop => prop.Value == ChartCategory.Distributed)
                                                   .ToProperty(this, vm => vm.IsDistributedChartCategoryActive);
            _chartTypes = this.ObservableForProperty(vm => vm.ChartCategory, skipInitial: false)
                                                   .Select(prop => prop.Value == ChartCategory.Cumulative
                                                            ? new ObservableCollection<string>(ChartTypeViewModelSource.GetCumulativeChartViewModels().Select(x => x.Description))
                                                            : new ObservableCollection<string>(ChartTypeViewModelSource.GetDistributedChartViewModels().Select(x => x.Description)))
                                                   .ToProperty(this, vm => vm.ChartTypes);
        }

        public string SourceDataFile
        {
            get
            {
                return _sourceDataFile;
            }
            set
            {                
                this.RaiseAndSetIfChanged(ref _sourceDataFile, value);
            }
        }

        public ChartCategory ChartCategory
        {
            get
            {
                return _chartCategory;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _chartCategory, value);
            }
        }

        public bool IsCumulativeChartCategoryActive => _isCumulativeChartCategoryActive.Value;

        public bool IsDistributedChartCategoryActive => _isDistributedChartCategoryActive.Value;

        public ObservableCollection<string> ChartTypes => _chartTypes.Value;

        public ChartTypeViewModel SelectedChartType
        {
            get
            {
                return _selectedChartType;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedChartType, value);
            }
        }

        public ObservableCollection<string> LifeEstimates { get; } = new ObservableCollection<string>();

        public ICommand OpenFileCommand { get; set; }
        public ICommand LoadChartCommand { get; set; }
    }
}
