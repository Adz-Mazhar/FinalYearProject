using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalYearProject.Converters
{
    class HexToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value is not string hex || !targetType.IsAssignableFrom(typeof(Color)))
            {
                return BindableProperty.UnsetValue;
            }

            return Color.FromHex(hex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value is not Color colour || !targetType.IsAssignableFrom(typeof(string)))
            {
                return null;
            }

            return colour.ToHex();
        }
    }
}
