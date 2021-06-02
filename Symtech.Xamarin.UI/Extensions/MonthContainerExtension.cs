using System.Collections.Generic;
using Symtech.Xamarin.UI.Controls.Api.Interfaces;
using Symtech.Xamarin.UI.Controls.Api.Models;

namespace Symtech.Xamarin.UI.Controls.Extensions
{
    internal static class MonthContainerExtension
    {
        public static void SelectDay(this MonthContainer monthContainer, Day selectedDay) => monthContainer._currentMonth.SelectDay(selectedDay);
        public static Day GetSelectedDay(this MonthContainer monthContainer) => monthContainer._currentMonth.GetSelectedDay();
        public static void AddEvents(this MonthContainer monthContainer, IEnumerable<ICalendarViewEvent> events) => monthContainer._currentMonth.AddEvents(events);

        public static void RemoveEvent(this MonthContainer monthContainer, ICalendarViewEvent calendarViewEvent) =>
            monthContainer._currentMonth.RemoveEvent(calendarViewEvent);

        public static void RemoveAllEvents(this MonthContainer monthContainer) =>
            monthContainer._currentMonth.RemoveAllEvents();
        
        public static string GetName(this MonthContainer monthContainer) => monthContainer._currentMonth.MonthDateTime.ToString("MMMM yyyy");
    }
}
