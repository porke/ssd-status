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
    internal class RawValueInfoViewModel : ReactiveObject
    {
        private ObservableAsPropertyHelper<string> _lastRefreshedCaption;
        private DateTime _lastRefreshed;

        public RawValueInfoViewModel()
        {
            _lastRefreshedCaption = this.ObservableForProperty(vm => vm.LastRefreshed, skipInitial: false)
                                        .Select(x => string.Format(Resources.RawValues,
                                                                   x.Value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)))
                                        .ToProperty(this, vm => vm.LastRefreshedCaption);
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

        public ICommand RefreshRawValues { get; set; }
    }
}
