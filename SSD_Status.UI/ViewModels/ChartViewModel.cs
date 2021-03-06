﻿using LiveCharts;
using System.Windows;
using ReactiveUI;
using System;
using System.Globalization;

namespace SSD_Status.WPF.ViewModels
{
    internal class ChartViewModel : ReactiveObject
    {        
        private string _YAxisTitle = "Gigabytes";
        private double _minimum = 0;
        private double _maximum = 1;        
        private string _seriesTitle = "None";
        private Visibility _chartVisibility = Visibility.Collapsed;
        private Func<double, string> _labelFormatter = x => x.ToString("0.##", CultureInfo.InvariantCulture);

        public string SeriesTitle
        {
            get { return _seriesTitle; }
            set { this.RaiseAndSetIfChanged(ref _seriesTitle, value); }
        }

        public string YAxisTitle
        {
            get { return _YAxisTitle; }
            set { this.RaiseAndSetIfChanged(ref _YAxisTitle, value); }
        }

        public double Minimum
        {
            get { return _minimum; }
            set { this.RaiseAndSetIfChanged(ref _minimum, value); }
        }

        public double Maximum
        {
            get { return _maximum; }
            set { this.RaiseAndSetIfChanged(ref _maximum, value); }
        }

        public Visibility ChartVisibility
        {
            get { return _chartVisibility; }
            set { this.RaiseAndSetIfChanged(ref _chartVisibility, value); }
        }

        public Func<double, string> LabelFormatter
        { 
            get { return _labelFormatter; }
            set { this.RaiseAndSetIfChanged(ref _labelFormatter, value); }
        }

        public ChartValues<double> SeriesValues { get; } = new ChartValues<double>();

        public ChartValues<string> Timestamps { get; } = new ChartValues<string>();
    }
}
