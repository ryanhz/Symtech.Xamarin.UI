﻿using System;
using System.Collections.Generic;
using System.Linq;
using Symtech.Xamarin.UI.Controls.Api.Interfaces;

namespace Symtech.Xamarin.UI.Controls.Api.Models
{
    internal class Day
    {
        private DateTime _currentDateTime;
        private bool _isCurrentMonth;
        public DateTime DateTime { get; }
        public IList<ICalendarViewEvent> Events { get; }

        public bool IsWeekend => DateTime.DayOfWeek switch
        {
            DayOfWeek.Saturday => true,
            DayOfWeek.Sunday => true,
            _ => false
        };

        public bool HasEvents => Events.Any();

        public Day(DateTime dateTime, bool isSelected = false, bool isCurrentMonth = true) : this(dateTime, DateTime.Now, isSelected, isCurrentMonth)
        {
        }

        public Day(DateTime dateTime, DateTime currentDateTime, bool isSelected = false, bool isCurrentMonth = true)
        {
            _currentDateTime = currentDateTime;
            _isCurrentMonth = isCurrentMonth;
            DateTime = dateTime;
            _isSelected = isSelected;
            Events = new List<ICalendarViewEvent>();
        }

        public bool IsToday => _currentDateTime.Date == DateTime.Date;

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        private bool _isIncluded;
        public bool IsIncluded
        {
            get => _isIncluded;
            set => _isIncluded = value;
        }

        public bool IsPreview => !_isCurrentMonth;

        public override bool Equals(object obj)
        {
            if (obj is Day dayToCompare)
                return dayToCompare.DateTime.Date.Ticks == DateTime.Date.Ticks;

            return false;
        }

        public override int GetHashCode() => DateTime.Date.Ticks.GetHashCode();

        public override string ToString() => DateTime.Day.ToString();
    }
}
