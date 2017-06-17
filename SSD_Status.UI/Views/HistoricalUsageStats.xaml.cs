using SSD_Status.WPF.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace SSD_Status.WPF.Views
{
    /// <summary>
    /// Interaction logic for UsageStatsInfo.xaml
    /// </summary>
    public partial class UsageStatsInfo : UserControl
    {
        public UsageStatsInfo()
        {
            InitializeComponent();

            var subscriptions = new CompositeDisposable
            {
                Observable.FromEventPattern<SelectionChangedEventArgs>(chartType, nameof(ComboBox.SelectionChanged))
                          .Subscribe(x => (DataContext as HistoricalUsageStatsViewModel).LoadChartCommand.Execute(null)),
                Observable.FromEventPattern<SelectionChangedEventArgs>(aggregationType, nameof(ComboBox.SelectionChanged))
                          .Subscribe(x => (DataContext as HistoricalUsageStatsViewModel).LoadChartCommand.Execute(null)),
            };

            Unloaded += (s, e) => subscriptions.Dispose();
        }
    }
}
