using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Symtech.Xamarin.UI.Controls.Extensions;
using Symtech.Xamarin.UI.Controls.Api.Interfaces;
using Symtech.Xamarin.UI.Controls.Api.Models;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;
using Symtech.Xamarin.UI.Controls.Api.Formatters;
using static Xamarin.Forms.Grid;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;

namespace Symtech.Xamarin.UI.Controls
{
    public partial class CalendarView : ContentView
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        private bool _isStartDayInitialized = false;
        private List<XView> _includedDays = new List<XView>();

        public static readonly BindableProperty IsHeaderVisibleProperty = BindableProperty.Create(nameof(IsHeaderVisible), typeof(bool), typeof(CalendarView), true, BindingMode.TwoWay,
             propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnHeaderVisibilityChanged));
        public static readonly BindableProperty IsContentVisibleProperty = BindableProperty.Create(nameof(IsContentVisible), typeof(bool), typeof(CalendarView), false, BindingMode.TwoWay,
             propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnContentVisibilityChanged));

        public bool IsHeaderVisible
        {
            get => (bool)GetValue(IsHeaderVisibleProperty);
            set => SetValue(IsHeaderVisibleProperty, value);
        }
        public bool IsContentVisible
        {
            get => (bool)GetValue(IsContentVisibleProperty);
            set => SetValue(IsContentVisibleProperty, value);
        }

        private static void OnHeaderVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView calendarView && newValue is bool isHeaderVisible)
            {
                calendarView.CalendarHeader.IsVisible = isHeaderVisible;
            }
        }
        private static void OnContentVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView calendarView && newValue is bool isContentVisible)
            {
                calendarView.CalendarContent.IsVisible = isContentVisible;
            }
        }


        public static BindableProperty FirstDayOfWeekProperty =
            BindableProperty.Create(
                nameof(FirstDayOfWeek),
                typeof(DayOfWeek),
                typeof(CalendarView),
                DayOfWeek.Sunday,
                BindingMode.OneTime);

        public DayOfWeek FirstDayOfWeek
        {
            get => (DayOfWeek)GetValue(FirstDayOfWeekProperty);
            set => SetValue(FirstDayOfWeekProperty, value);
        }

        public static BindableProperty IsPreviewDaysActiveProperty =
            BindableProperty.Create(
                nameof(IsPreviewDaysActive),
                typeof(bool),
                typeof(CalendarView),
                false,
                BindingMode.OneTime);

        public bool IsPreviewDaysActive
        {
            get => (bool)GetValue(IsPreviewDaysActiveProperty);
            set => SetValue(IsPreviewDaysActiveProperty, value);
        }

        public static BindableProperty DaysOfWeekFormatterProperty =
            BindableProperty.Create(
                nameof(DaysOfWeekFormatter),
                typeof(IDayOfWeekFormatter),
                typeof(CalendarView),
                new DayOfWeek3CaractersFormat(),
                BindingMode.OneTime);

        public IDayOfWeekFormatter DaysOfWeekFormatter
        {
            get => (IDayOfWeekFormatter)GetValue(DaysOfWeekFormatterProperty);
            set => SetValue(DaysOfWeekFormatterProperty, value);
        }

        public static BindableProperty ThemeProperty =
            BindableProperty.Create(
                nameof(Theme),
                typeof(ResourceDictionary),
                typeof(CalendarView),
                new DefaultTheme(),
                BindingMode.OneTime);

        public ResourceDictionary Theme
        {
            get => (ResourceDictionary)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        public event Action<MonthRange>? MonthChanged;
        public event Action<DateRangeSelected>? DaySelected;

        public static BindableProperty SelectionFinishedProperty =
            BindableProperty.Create(
                nameof(SelectionFinished),
                typeof(Action),
                typeof(CalendarView),
                null,
                BindingMode.TwoWay);
        public event Action? SelectionFinished;
        public static BindableProperty SelectionStartedProperty =
            BindableProperty.Create(
                nameof(SelectionStarted),
                typeof(Action),
                typeof(CalendarView),
                null,
                BindingMode.TwoWay);
        public event Action? SelectionStarted;

        private MonthContainer? _monthContainer;
        private int _numberOfDaysInContainer;

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == "IsBottomBarVisible")
            {
                if (CalendarContent.IsVisible == false)
                {
                    CalendarContent.IsVisible = true;
                    MonthPicker.IsVisible = true;
                    CalendarIcon.TextColor = (Color)Application.Current.Resources["PrimaryColor"];
                    SelectionStarted?.Invoke();
                }
                else
                {
                    CalendarContent.IsVisible = false;
                    MonthPicker.IsVisible = false;
                    CalendarIcon.TextColor = Color.FromHex("#848690");
                    SelectionFinished?.Invoke();
                }
            }
            if (propertyName == "Renderer")
            {
                Resources.MergedDictionaries.Add(Theme);
                _monthContainer = new MonthContainer(DateTime.Today, DaysOfWeekFormatter, FirstDayOfWeek, IsPreviewDaysActive);

                var days = _monthContainer.Days;
                _numberOfDaysInContainer = days.Count;

                var column = 0;
                var row = 0;

                foreach (var _ in days)
                {
                    var calendarDay = new CalendarDay();
                    calendarDay.DaySelected += CalendarDayOnDaySelected;
                    CalendarDaysContainer.Children.Add(calendarDay, column, row);

                    if (++column > 6)
                    {
                        column = 0;
                        row++;
                    }
                }
                RecycleDays(days);
                RefreshSelectedRange();
                RemoveAllBlankDays();
                UpdateDateLabel();

                column = 0;

                foreach (var dayOfWeek in _monthContainer.DaysOfWeek)
                {
                    var calendarDayOfWeek = new CalendarDayOfWeek();
                    CalendarDaysOfWeekContainer.Children.Add(calendarDayOfWeek, column++, 0);
                    calendarDayOfWeek.UpdateData(dayOfWeek);
                }

                MonthName.Text = _monthContainer.GetName();
                MonthChanged?.Invoke(new MonthRange(_monthContainer.FirstDay, _monthContainer.LastDay));
            }
        }

        private void RemoveAllBlankDays()
        {
            var BlankDays = CalendarDaysContainer.Children.Where(d => ((CalendarDay)d).Day == null).ToList();
            foreach(var day in BlankDays)
            {
                CalendarDaysContainer.Children.Remove(day);
            }
        }

        private void UpdateDateLabel()
        {
            if (StartDate != null && EndDate == null)
            {
                DateRangeLabel.Text = $"{StartDate.Value.Day} {StartDate?.ToString("MMMM")}";
            }
            else if (StartDate != null && EndDate != null)
            {
                DateRangeLabel.Text = $"{StartDate?.Day} {StartDate?.ToString("MMMM")} - {EndDate?.Day} {EndDate?.ToString("MMMM")}";
            }
            else
            {
                DateRangeLabel.Text = "Please select a date";
            }
        }

        private void CalendarDayOnDaySelected(CalendarDay? calendarDay)
        {
            if (calendarDay?.Day == null)
                return;

            if (StartDate != null && EndDate != null)
            {
                _isStartDayInitialized = true;
                if (StartDate < calendarDay.Day.DateTime && EndDate > calendarDay.Day.DateTime)
                {
                    ClearAllSelection();
                    UpdateDateLabel();
                    return;
                }
            }

            if (_includedDays.Count != 0)
            {
                foreach (var day in _includedDays)
                {
                    ((CalendarDay)day).UnSelect();
                }
                _includedDays.Clear();
            }

            if(calendarDay.Day.DateTime == StartDate)
            {
                UnselectDay(StartDate);
                if (EndDate != null)
                {
                    StartDate = EndDate;
                    EndDate = null;
                }
                else
                {
                    StartDate = null;
                    _isStartDayInitialized = false;
                }
                UpdateDateLabel();
                return;
            }
            else if (calendarDay.Day.DateTime == EndDate)
            {
                UnselectDay(EndDate);
                EndDate = null;
                UpdateDateLabel();
                return;
            }

            if (_isStartDayInitialized == false)
            {
                calendarDay.Select();
                StartDate = calendarDay.Day.DateTime;
                _isStartDayInitialized = true;
                DaySelected?.Invoke(new DateRangeSelected(StartDate, null));
            }
            else
            {
                if (EndDate == null)
                {
                    EndDate = calendarDay.Day.DateTime;
                }
                else
                {
                    if (calendarDay.Day.DateTime < StartDate)
                    {
                        UnselectDay(StartDate);
                        StartDate = calendarDay.Day.DateTime;
                    }
                    else if (calendarDay.Day.DateTime > EndDate)
                    {
                        UnselectDay(EndDate);
                        EndDate = calendarDay.Day.DateTime;
                    }
                }
                calendarDay.Select();

                if (StartDate > EndDate)
                {
                    var tempDate = StartDate;
                    StartDate = EndDate;
                    EndDate = tempDate;
                }
                _includedDays = CalendarDaysContainer.Children.Where(x => ((CalendarDay)x).Day?.DateTime > StartDate && ((CalendarDay)x).Day?.DateTime < EndDate).ToList();
                foreach (var day in _includedDays)
                {
                    ((CalendarDay)day).Include();
                }
                DaySelected?.Invoke(new DateRangeSelected(StartDate, EndDate));
            }
            UpdateDateLabel();
        }

        private void ClearAllSelection()
        {
            UnselectDay(StartDate);
            UnselectDay(EndDate);
            StartDate = null;
            EndDate = null;
            foreach (var day in _includedDays)
            {
                ((CalendarDay)day).UnSelect();
            }
            _includedDays.Clear();
            _isStartDayInitialized = false;
        }

        public CalendarView()
        {
            InitializeComponent();
            CalendarHeader.IsVisible = IsHeaderVisible;
            CalendarContent.IsVisible = IsContentVisible;
        }

        private async void OnPreviousMonthClick(object sender, EventArgs e)
        {
            var result = await Task.Run(() =>
            {
                if (_monthContainer is null)
                    return default;

                _monthContainer.Previous();

                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();
                var firstDay = _monthContainer.FirstDay;
                var lastDay = _monthContainer.LastDay;

                return (days, monthName, firstDay, lastDay);
            });

            if (result.Equals(default))
                return;

            MonthName.Text = result.monthName;
            RecycleDays(result.days);
            RefreshSelectedRange();
            MonthChanged?.Invoke(new MonthRange(result.firstDay, result.lastDay));
        }

        private async void OnNextMonthClick(object sender, EventArgs e)
        {
            var result = await Task.Run(() =>
            {
                if (_monthContainer is null)
                    return default;

                _monthContainer.Next();

                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();
                var firstDay = _monthContainer.FirstDay;
                var lastDay = _monthContainer.LastDay;

                return (days, monthName, firstDay, lastDay);
            });

            if (result.Equals(default))
                return;

            MonthName.Text = result.monthName;
            RecycleDays(result.days);
            RefreshSelectedRange();
            MonthChanged?.Invoke(new MonthRange(result.firstDay, result.lastDay));
        }

        private void UnselectDay(DateTime? dateTime)
        {
            if(dateTime != null)
            {
                var dayView = (CalendarDay)CalendarDaysContainer.Children.SingleOrDefault(x => ((CalendarDay)x).Day?.DateTime == dateTime);
                if (dayView != null)
                {
                    dayView.UnSelect();
                }
            }
        }

        private void SelectDay(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var dayView = (CalendarDay)CalendarDaysContainer.Children.SingleOrDefault(x => ((CalendarDay)x).Day?.DateTime == dateTime);
                if (dayView != null)
                {
                    dayView.Select();
                }
            }
        }

        private void RefreshSelectedRange()
        {
            if(StartDate != null && EndDate != null)
            {
                _includedDays = CalendarDaysContainer.Children.Where(x => ((CalendarDay)x).Day?.DateTime > StartDate && ((CalendarDay)x).Day?.DateTime < EndDate).ToList();
                foreach (var day in _includedDays)
                {
                    ((CalendarDay)day).Include();
                }

                SelectDay(StartDate);
                SelectDay(EndDate);
            }
            else
            {
                SelectDay(StartDate);
            }
        }

        private void RecycleDays(IReadOnlyList<Day?> days)
        {
            for (var index = 0; index < _numberOfDaysInContainer; index++)
            {
                var day = days[index];

                if (CalendarDaysContainer.Children[index] is CalendarDay view)
                    view.UpdateData(day);
            }
        }

        private void OnCalendarTapped(object sender, EventArgs e)
        {
            if(CalendarContent.IsVisible == false)
            {
                CalendarContent.IsVisible = true;
                MonthPicker.IsVisible = true;
                CalendarIcon.TextColor = (Color)Application.Current.Resources["PrimaryColor"];
                SelectionStarted?.Invoke();
            }
            else
            {
                CalendarContent.IsVisible = false;
                MonthPicker.IsVisible = false;
                CalendarIcon.TextColor = Color.FromHex("#848690");
                SelectionFinished?.Invoke();
            }
        }
    }

    public readonly struct MonthRange
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public MonthRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }

    public readonly struct DateRangeSelected
    {
        public DateTime? StartDateTime { get; }
        public DateTime? EndDateTime { get; }

        public DateRangeSelected(DateTime? startDateTime, DateTime? endDateTime)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }
    }
}

