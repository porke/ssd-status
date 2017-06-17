using SSD_Status.WPF.ViewModels.Sources;
using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using SSD_Status.WPF.ViewModels.Enums;

namespace SSD_Status.WPF.ViewModels.Converters
{
    internal class ChartTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            var viewModel = value as EnumerableViewModel<ChartType>;
            return viewModel.Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var desc = value as string;
            var returnValue = ChartTypeViewModelSource.GetCumulativeChartViewModels()
                .Concat(ChartTypeViewModelSource.GetDistributedChartViewModels())
                .FirstOrDefault(x => x.Description == desc);

            return returnValue ?? ChartTypeViewModelSource.GetDistributedChartViewModels().First();
        }
    }
}
