using SSD_Status.WPF.ViewModels.Sources;
using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;

namespace SSD_Status.WPF.ViewModels.Converters
{
    internal class ChartTypeViewModelToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var viewModel = value as ChartTypeViewModel;
            return viewModel.Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var description = value as string;
            return ChartTypeViewModelSource.GetChartViewModelTypes().First(x => x.Description == description);
        }
    }
}
