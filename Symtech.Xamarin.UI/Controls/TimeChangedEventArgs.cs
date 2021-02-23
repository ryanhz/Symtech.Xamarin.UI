using System;

namespace Symtech.Xamarin.UI.Controls
{
    public class TimeChangedEventArgs : EventArgs
    {
        public TimeSpan OldValue { get; set; }
        public TimeSpan NewValue { get; set; }

        public TimeChangedEventArgs(TimeSpan oldValue, TimeSpan newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}
