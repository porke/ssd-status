using System.Collections.ObjectModel;

namespace SSD_Status.WPF.ViewModels
{
    internal class UsageStatsInfoViewModel
    {
        public string SourceDataFile { get; set; } = "SomeFileVeryFarAway.csv";

        public ObservableCollection<string> LifeEstimates
        {
            get
            {
                return new ObservableCollection<string> { "Not", "Very", "Long" };
            }
        }
    }
}
