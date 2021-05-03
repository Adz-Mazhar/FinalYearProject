using Plugin.CloudFirestore;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalYearProject.Converters
{
    public class TimestampToDateAndTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value is not Timestamp timestamp || !targetType.IsAssignableFrom(typeof(string)))
            {
                return BindableProperty.UnsetValue;
            }

            var localTime = timestamp.ToDateTime().ToLocalTime();
            return $"{localTime:M}, {localTime:t}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
