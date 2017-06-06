using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SSD_Status.WPF.Utilities;

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

        public double Minimum => UsageValues.Select(x => x.Value).Min();

        public double Maximum => UsageValues.Select(x => x.Value).Max();

        public RangeObservableCollection<KeyValuePair<DateTime, double>> UsageValues { get; } = new RangeObservableCollection<KeyValuePair<DateTime, double>>();
    }
}
