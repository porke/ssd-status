using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SSD_Status.WPF.ViewModels
{
    internal class ChartViewModel : ViewModelBase
    {
        private string _title = "Usage chart";
        private string _seriesTitle = "Usage";
        private string _YAxisTitle = "Gigabytes";

        public string SeriesTitle
        {
            get
            {
                return _seriesTitle;
            }
            set
            {
                _seriesTitle = value;
                NotifyPropertyChanged(nameof(SeriesTitle));
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged(nameof(Title));
            }
        }

        public string YAxisTitle
        {
            get
            {
                return _YAxisTitle;
            }
            set
            {
                _YAxisTitle = value;
                NotifyPropertyChanged(nameof(YAxisTitle));
            }
        }

        public ObservableCollection<KeyValuePair<DateTime, double>> UsageValues { get; } = new ObservableCollection<KeyValuePair<DateTime, double>>();
    }
}
