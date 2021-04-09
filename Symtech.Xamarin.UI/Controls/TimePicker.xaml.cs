using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Symtech.Xamarin.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimePicker : ContentView
    {
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TimePicker), default(Color), BindingMode.OneWay, null);
        public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan), typeof(TimePicker), (object)new TimeSpan(0L), BindingMode.TwoWay, (BindableProperty.ValidateValueDelegate)((bindable, value) =>
        {
            TimeSpan timeSpan = (TimeSpan)value;
            return timeSpan.TotalHours < 24.0 && timeSpan.TotalMilliseconds >= 0.0;
        }), propertyChanged: TimePropertyChanged);

        public event EventHandler<TimeChangedEventArgs> TimeSelected;

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public TimeSpan Time
        {
            get => (TimeSpan)this.GetValue(TimeProperty);
            set
            {
                this.SetValue(TimeProperty, value);
            }
        }

        public TimePicker()
        {
            InitializeComponent();
        }

        void OnTimeEntryTapped(System.Object sender, System.EventArgs e)
        {
            ShowTimePicker();
        }

        void OnClockButtonClicked(System.Object sender, System.EventArgs e)
        {
            ShowTimePicker();
        }

        private static void TimePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            EventHandler<TimeChangedEventArgs> timeSelected = ((TimePicker)bindable).TimeSelected;
            if (timeSelected == null)
                return;
            timeSelected(bindable, new TimeChangedEventArgs((TimeSpan)oldValue, (TimeSpan)newValue));
        }

        private void ShowTimePicker()
        {
            //if (timePicker.IsFocused)
            //{
            //    timePicker.Unfocus();
            //}
            timePicker.Focus();
        }

    }

}
