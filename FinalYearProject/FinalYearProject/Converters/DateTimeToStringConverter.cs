using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalYearProject.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value is not DateTime dateTime || !targetType.IsAssignableFrom(typeof(string)))
            {
                return BindableProperty.UnsetValue;
            }

            var localTime = dateTime.ToLocalTime();
            return $"{localTime:M}, {localTime:t}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
