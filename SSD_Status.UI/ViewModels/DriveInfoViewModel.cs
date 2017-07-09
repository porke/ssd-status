using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SSD_Status.WPF.ViewModels
{
    internal class DriveInfoViewModel : ReactiveObject
    {
        private IDisposable _refreshDriveBinding;

        public DriveInfoViewModel()
        {
            _refreshDriveBinding = this.WhenAnyValue(x => x.SelectedDrive)
                                       .Subscribe(_ => RefreshDriveInfoCommand?.Execute(null));
        }

        private string _selectedDrive = string.Empty;

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

        public ObservableCollection<string> Drives { get; set; } = new ObservableCollection<string>();

        public ICommand RefreshDriveInfoCommand { get; set; }
    }
}
