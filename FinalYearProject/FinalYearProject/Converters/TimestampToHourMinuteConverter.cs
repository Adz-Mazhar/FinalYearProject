using Plugin.CloudFirestore;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalYearProject.Converters
{
    public class TimestampToHourMinuteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value is not Timestamp timestamp || !targetType.IsAssignableFrom(typeof(string)))
            {
                return BindableProperty.UnsetValue;
            }

            return timestamp.ToDateTime().ToLocalTime().ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
