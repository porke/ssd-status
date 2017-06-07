using System;
using LiveCharts;
using System.Windows;

namespace SSD_Status.WPF.ViewModels
{
    internal class ChartViewModel : ViewModelBase
    {        
        private string _YAxisTitle = "Gigabytes";
        private double _minimum = 0;
        private double _maximum = 1;        
        private string _seriesTitle = "None";
        private Visibility _chartVisibility = Visibility.Collapsed;

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

        public double Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
                NotifyPropertyChanged(nameof(Minimum));
            }
        }

        public double Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
                NotifyPropertyChanged(nameof(Maximum));
            }
        }

        public Visibility ChartVisibility
        {
            get
            {
                return _chartVisibility;
            }
            set
            {
                _chartVisibility = value;
                NotifyPropertyChanged(nameof(ChartVisibility));
            }
        }

        public ChartValues<double> SeriesValues { get; } = new ChartValues<double>();

        public ChartValues<string> Timestamps { get; } = new ChartValues<string>();
    }
}
