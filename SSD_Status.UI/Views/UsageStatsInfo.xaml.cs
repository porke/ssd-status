using System.Collections.Generic;
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

            var valueList = new List<KeyValuePair<int, int>>
            {
                new KeyValuePair<int, int>(1, 60),
                new KeyValuePair<int, int>(2, 20),
                new KeyValuePair<int, int>(3, 50),
                new KeyValuePair<int, int>(4, 30),
                new KeyValuePair<int, int>(5, 40)
            };

            //Setting data for line chart
            UsageChart.DataContext = valueList;
        }
    }
}
