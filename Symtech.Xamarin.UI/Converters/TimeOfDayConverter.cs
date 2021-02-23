using System;
using System.Globalization;
using Xamarin.Forms;

namespace Symtech.Xamarin.UI.Converters
{
    public class TimeOfDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.ToString(@"hh\:mm");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return TimeSpan.Parse(s);
            }
            return value;
        }
    }

}
