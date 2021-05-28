using Symtech.Xamarin.UI.Controls.Api.Interfaces;
using Symtech.Xamarin.UI.Controls.Api.Models;

namespace Symtech.Xamarin.UI.Controls.Extensions
{
    internal static class DayExtension
    {
        public static void AddEvent(this Day day, ICalendarViewEvent @event)
        {
            day.Events.Add(@event);
        }

        public static void RemoveEvent(this Day day, ICalendarViewEvent @event)
        {
            day.Events.Remove(@event);
        }

        public static void RemoveAllEvents(this Day day)
        {
            day.Events.Clear();
        }
    }
}
