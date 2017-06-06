using SSD_Status.WPF.ViewModels;
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
        }

        private void ChartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as UsageStatsInfoViewModel).LoadChartCommand.Execute(e.AddedItems);
        }
    }
}
