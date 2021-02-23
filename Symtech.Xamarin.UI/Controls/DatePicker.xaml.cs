using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Symtech.Xamarin.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatePicker : ContentView
    {
        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(DatePicker), default(DateTime), BindingMode.TwoWay, null, propertyChanged: DatePropertyChanged, propertyChanging: null, defaultValueCreator: (BindableObject bindable) => DateTime.Today);

        public event EventHandler<DateChangedEventArgs> DateSelected;

        public DateTime Date
        {
            get => (DateTime)this.GetValue(DateProperty);
            set
            {
                this.SetValue(DateProperty, value);
            }
        }

        public DatePicker()
        {
            InitializeComponent();
        }

        void OnDateEntryTapped(System.Object sender, System.EventArgs e)
        {
            ShowDatePicker();
        }

        void OnCalendarButtonClicked(System.Object sender, System.EventArgs e)
        {
            ShowDatePicker();
        }

        private static void DatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            EventHandler<DateChangedEventArgs> dateSelected = ((DatePicker)bindable).DateSelected;
            if (dateSelected == null)
                return;
            dateSelected(bindable, new DateChangedEventArgs((DateTime)oldValue, (DateTime)newValue));
        }

        private void ShowDatePicker()
        {
            if (datePicker.IsFocused)
            {
                datePicker.Unfocus();
            }
            datePicker.Focus();
        }

        void OnDateSelected(System.Object sender, DateChangedEventArgs e)
        {
            datePicker.Unfocus();
            DateEntry.Unfocus();
        }

    }

}
