using SSD_Status.WPF.ViewModels;
using System;
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
            
            var subscription = Observable.FromEventPattern<SelectionChangedEventArgs>(chartType, nameof(ComboBox.SelectionChanged))
                                         .Subscribe(x => (DataContext as HistoricalUsageStatsViewModel).LoadChartCommand.Execute(null));
            Unloaded += (s, e) => subscription.Dispose();
        }
    }
}
