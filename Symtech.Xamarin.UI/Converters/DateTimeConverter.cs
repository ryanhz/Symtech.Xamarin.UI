using System;
using System.Globalization;
using Xamarin.Forms;

namespace Symtech.Xamarin.UI.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if(dateTime == DateTime.MinValue)
                {
                    return "Never";
                }
                return dateTime.ToString("G");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return DateTime.Parse(s);
            }
            return value;
        }
    }

}
