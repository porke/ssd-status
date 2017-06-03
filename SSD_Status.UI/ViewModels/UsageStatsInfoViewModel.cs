using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SSD_Status.WPF.ViewModels
{
    internal class UsageStatsInfoViewModel : ViewModelBase
    {
        private string _sourceDataFile = "SomeFileVeryFarAway.csv";

        public string SourceDataFile
        {
            get
            {
                return _sourceDataFile;
            }
            set
            {
                _sourceDataFile = value;
                NotifyPropertyChanged(nameof(SourceDataFile));
            }
        }

        public ObservableCollection<string> LifeEstimates { get; } = new ObservableCollection<string>();
        
        public ObservableCollection<KeyValuePair<DateTime, double>> UsageValues { get; } = new ObservableCollection<KeyValuePair<DateTime, double>>();

        public ICommand OpenFileCommand { get; set; }      
    }
}
