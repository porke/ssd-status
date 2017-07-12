using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SSD_Status.WPF.Properties;
using System.Globalization;

namespace SSD_Status.WPF.ViewModels
{
    internal class DriveInfoViewModel : ReactiveObject
    {
        private IDisposable _refreshDriveBinding;

        private ObservableAsPropertyHelper<string> _lastRefreshedCaption;
        private DateTime _lastRefreshed;
        private string _selectedDrive = string.Empty;

        public DriveInfoViewModel()
        {
            _refreshDriveBinding = this.WhenAnyValue(x => x.SelectedDrive)
                                       .Subscribe(_ => RefreshDriveInfoCommand?.Execute(null));

            _lastRefreshedCaption = this.ObservableForProperty(vm => vm.LastRefreshed, skipInitial: false)
                                        .Select(x => string.Format(Resources.RawValues,
                                                                   x.Value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)))
                                        .ToProperty(this, vm => vm.LastRefreshedCaption);
        }        

        public ObservableCollection<GridPropertyViewModel> DriveInfo { get; set; } = new ObservableCollection<GridPropertyViewModel>();

        public string SelectedDrive
        {
            get
            {
                return _selectedDrive;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDrive, value);
            }
        }        

        public ObservableCollection<GridPropertyViewModel> RawValues { get; } = new ObservableCollection<GridPropertyViewModel>();

        public DateTime LastRefreshed
        {
            get
            {
                return _lastRefreshed;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _lastRefreshed, value);
            }
        }

        public string LastRefreshedCaption => _lastRefreshedCaption.Value;
        public ObservableCollection<string> Drives { get; set; } = new ObservableCollection<string>();

        public ICommand RefreshRawValues { get; set; }
        public ICommand RefreshDriveInfoCommand { get; set; }
    }
}
