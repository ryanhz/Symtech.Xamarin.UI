using System;
using System.Globalization;
using Symtech.Xamarin.UI.Controls.Api.Interfaces;

namespace Symtech.Xamarin.UI.Controls.Api.Formatters
{
    public class DayOfWeek3CaractersFormat : IDayOfWeekFormatter
    {
        public string Format(DayOfWeek dayOfWeek)
        {
            return CultureInfo
                .CurrentCulture
                .DateTimeFormat
                .GetDayName(dayOfWeek)
                .Substring(0, 3)
                .ToLower();
        }
    }
}
