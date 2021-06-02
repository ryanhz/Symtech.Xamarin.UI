using System;

namespace Symtech.Xamarin.UI.Controls.Api.Interfaces
{
    public interface ICalendarViewEvent
    {
        object Id { get; }
        string Name { get; }
        DateTime StartDateTime { get; }
        DateTime EndDateTime { get; }
        bool IsAllDay { get; }
    }
}
