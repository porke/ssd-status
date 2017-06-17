using SSD_Status.WPF.ViewModels.Sources;
using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using SSD_Status.WPF.ViewModels.Enums;

namespace SSD_Status.WPF.ViewModels.Converters
{
    internal class AggregationTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var viewModel = value as EnumerableViewModel<AggregationType>;
            return viewModel.Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var desc = value as string;
            return AggregationTypeViewModelSource.GetAggregationTypes()
                .First(x => x.Description == desc);
        }
    }
}
