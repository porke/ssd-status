﻿using ReactiveUI;
using System.Windows.Input;
using System.Reactive.Linq;
using SSD_Status.WPF.Properties;
using System.Collections.ObjectModel;

namespace SSD_Status.WPF.ViewModels
{
    internal class RealTimeUsageViewModel : ReactiveObject
    {
        private ObservableAsPropertyHelper<string> _toggleButtonCaption;
        private bool _isEnabled = false;
        private bool _startFromZero = true;

        public ChartViewModel ChartViewModel { get; } = new ChartViewModel();

        public RealTimeUsageViewModel()
        {
            _toggleButtonCaption = this.ObservableForProperty(vm => vm.IsEnabled, skipInitial: false)
                                        .Select(x => x.Value ? Resources.Disable : Resources.Enable)
                                        .ToProperty(this, vm => vm.ToggleButtonCaption);
        }
        
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { this.RaiseAndSetIfChanged(ref _isEnabled, value); }
        }

        public bool StartFromZero
        {
            get { return _startFromZero; }
            set { this.RaiseAndSetIfChanged(ref _startFromZero, value); }
        }

        public ObservableCollection<EntryViewModel> DataEntries { get; } = new ObservableCollection<EntryViewModel>();

        public string ToggleButtonCaption => _toggleButtonCaption.Value;

        public ICommand ToggleStartFromZero { get; set; }
        public ICommand ExportReadingsCommand { get; set; }
        public ICommand ToggleMonitoringCommand { get; set; }
    }
}
