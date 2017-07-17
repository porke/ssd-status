using System;
using System.Globalization;
using System.Windows.Data;

namespace SSD_Status.WPF.ViewModels.Converters
{
    internal class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = (bool)value;
            return !booleanValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = (bool)value;
            return !booleanValue;
        }
    }
}
