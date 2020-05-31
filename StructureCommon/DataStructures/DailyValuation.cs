﻿using System;
using StructureCommon.Extensions;

namespace StructureCommon.DataStructures
{
    /// <summary>
    /// Holds a date and a value to act as the value on that day.
    /// </summary>
    public class DailyValuation : IComparable
    {
        /// <summary>
        /// Appends date in UK format with value, separated by a comma.
        /// </summary>
        public override string ToString()
        {
            return Day.ToUkDateStringPadded() + ", " + Value.ToString();
        }

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            DailyValuation a = (DailyValuation)obj;
            return DateTime.Compare(Day, a.Day);
        }

        /// <summary>
        /// Returns a copy of the specified valuation
        /// </summary>
        /// <returns></returns>
        public DailyValuation Copy()
        {
            return new DailyValuation(Day, Value);
        }

        /// <summary>
        /// Sets the data in the daily valuation.
        /// </summary>
        public void SetData(DateTime date, double value)
        {
            Day = date;
            Value = value;
        }

        /// <summary>
        /// Sets the day field only.
        /// </summary>
        public void SetDay(DateTime date)
        {
            Day = date;
        }

        /// <summary>
        /// Sets the value field.
        /// </summary>
        public void SetValue(double value)
        {
            Value = value;
        }

        /// <summary>
        /// The date stored in this valuation.
        /// </summary>
        protected DateTime fDate;

        /// <summary>
        /// The date for the valuation
        /// </summary>
        public virtual DateTime Day
        {
            get
            {
                return fDate;
            }
            set
            {
                fDate = value;
            }
        }

        /// <summary>
        /// The value stored in this valuation.
        /// </summary>
        protected double fValue;

        /// <summary>
        /// The specific valuation
        /// </summary>
        public virtual double Value
        {
            get
            {
                return fValue;
            }
            set
            {
                fValue = value;
            }
        }

        /// <summary>
        /// empty constructor.
        /// </summary>
        public DailyValuation()
        {
        }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public DailyValuation(DateTime idealDate, double idealValue)
        {
            Day = idealDate;
            Value = idealValue;
        }

        /// <summary>
        /// Creates a new <see cref="DailyValuation"/> from the given one.
        /// </summary>
        public DailyValuation(DailyValuation dailyValue)
            : this(dailyValue.Day, dailyValue.Value)
        {
        }
    }
}
