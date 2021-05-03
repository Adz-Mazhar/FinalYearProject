using FinalYearProject.Extensions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalYearProject.Converters
{
    class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value is not Enum @enum || !targetType.IsAssignableFrom((typeof(string))))
            {
                return BindableProperty.UnsetValue;
            }

            return @enum.ToString().AddSpaceBeforeCapitalLetters();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }


    }
}
