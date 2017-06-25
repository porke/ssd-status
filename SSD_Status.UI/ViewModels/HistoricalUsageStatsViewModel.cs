using System.Linq;
using SSD_Status.WPF.ViewModels.Sources;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using System.Reactive.Linq;
using SSD_Status.WPF.ViewModels.Enums;
using System;

namespace SSD_Status.WPF.ViewModels
{
    internal class HistoricalUsageStatsViewModel : ReactiveObject
    {
        private string _sourceDataFile = "SomeFileVeryFarAway.csv";
        private EnumerableViewModel<ChartType> _selectedChartType = ChartTypeViewModelSource.GetCumulativeChartViewModels().First();
        private EnumerableViewModel<AggregationType> _selectedAggregationType = AggregationTypeViewModelSource.GetAggregationTypes().First();
        private ChartCategory _chartCategory = ChartCategory.Cumulative;
        private int _movingAveragePeriod = 30;

        private ObservableAsPropertyHelper<bool> _isCumulativeChartCategoryActive;
        private ObservableAsPropertyHelper<bool> _isDistributedChartCategoryActive;
        private ObservableAsPropertyHelper<ObservableCollection<string>> _chartTypes;
        private IDisposable _chartUpdateBinding;

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

            _chartUpdateBinding = this.WhenAnyValue(x => x.SelectedChartType,
                                                    x => x.SourceDataFile,
                                                    x => x.ChartCategory,
                                                    x => x.MovingAveragePeriod,
                                                    x => x.SelectedAggregationType)
                                      .Subscribe(_ => RefreshChartCommand?.Execute(null));
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

        public int MovingAveragePeriod
        {
            get
            {
                return _movingAveragePeriod;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _movingAveragePeriod, value);
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

        public EnumerableViewModel<ChartType> SelectedChartType
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

        public EnumerableViewModel<AggregationType> SelectedAggregationType
        {
            get
            {
                return _selectedAggregationType;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedAggregationType, value);
            }
        }

        public ObservableCollection<string> AggregationTypes
        {
            get
            {
                return new ObservableCollection<string>(AggregationTypeViewModelSource.GetAggregationTypes().Select(x => x.Description));
            }
        }

        public ObservableCollection<string> LifeEstimates { get; } = new ObservableCollection<string>();

        public ICommand OpenFileCommand { get; set; }
        public ICommand RefreshChartCommand { get; set; }
    }
}
