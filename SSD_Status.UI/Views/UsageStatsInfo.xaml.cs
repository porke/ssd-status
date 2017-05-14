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

            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            valueList.Add(new KeyValuePair<string, int>("Developer", 60));
            valueList.Add(new KeyValuePair<string, int>("Misc", 20));
            valueList.Add(new KeyValuePair<string, int>("Tester", 50));
            valueList.Add(new KeyValuePair<string, int>("QA", 30));
            valueList.Add(new KeyValuePair<string, int>("Project Manager", 40));

            //Setting data for line chart
            UsageChart.DataContext = valueList;
        }
    }
}
