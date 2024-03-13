using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.MathLibrary.Finance;

namespace Effanville.Common.Structure.DataStructures.Numeric
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    public partial class TimeNumberList
    {

        /// <inheritdoc />
        public TimeNumberList Inverted() => Inverted(Values());

        private static TimeNumberList Inverted(List<DailyNumeric> values)
        {
            TimeNumberList inverted = new TimeNumberList();
            if (values.Count > 0)
            {
                foreach (DailyNumeric value in values)
                {
                    if (!value.Value.Equals(0))
                    {
                        inverted.SetData(value.Day, 1 / value.Value);
                    }
                    else
                    {
                        inverted.SetData(value.Day, double.PositiveInfinity);
                    }
                }
            }

            return inverted;
        }

        /// <summary>
        /// Adds all values in the list. The sum of an empty list is defined to be 0.
        /// </summary>
        public double Sum() => Sum(Values());

        /// <summary>
        /// Adds all values in the list that satisfy the predicate.
        /// </summary>
        public double Sum(Func<DailyNumeric, bool> predicate) => Sum(Values().Where(val => predicate(val)));

        private static double Sum(IEnumerable<DailyNumeric> values) => values.Sum(val => val.Value);

        /// <summary>
        /// returns the CAR of the timelist between the dates provided.
        /// </summary>
        public double CAR(DateTime earlierTime, DateTime laterTime) => CAR(Values(), earlierTime, laterTime);

        private static double CAR(List<DailyNumeric> values, DateTime earlierTime, DateTime laterTime)
        {
            DailyNumeric earlierValue = Value(values, earlierTime, OutlierInterpolation, OutlierInterpolation, DayBasedInterpolationFunction);
            DailyNumeric laterValue = Value(values, laterTime, OutlierInterpolation, OutlierInterpolation, DayBasedInterpolationFunction);
            if (earlierValue == null || laterValue == null)
            {
                return double.NaN;
            }

            return FinanceFunctions.CAR(earlierValue, laterValue);
        }

        /// <summary>
        /// returns the CAR of the timelist between the dates provided.
        /// </summary>
        public double CAROnOrBefore(DateTime earlierTime, DateTime laterTime)
            => CAROnOrBefore(Values(), earlierTime, laterTime);

        private static double CAROnOrBefore(List<DailyNumeric> values, DateTime earlierTime, DateTime laterTime)
        {
            DailyNumeric earlierValue = Value(values, earlierTime, (a, b) => null, OutlierInterpolation, (a, b, c) => a);
            DailyNumeric laterValue = Value(values, laterTime, (a, b) => null, OutlierInterpolation, (a, b, c) => a); ;
            if (earlierValue == null || laterValue == null)
            {
                return double.NaN;
            }

            return FinanceFunctions.CAR(earlierValue, laterValue);
        }

        /// <summary>
        /// Returns internal rate of return of the values in the TimeList
        /// </summary>
        internal double IRR(DailyNumeric latestValue) => IRR(Values(), latestValue);

        private static double IRR(List<DailyNumeric> values, DailyNumeric latestValue)
        {
            if (values == null || values.Count == 0 || latestValue == null)
            {
                return double.NaN;
            }

            // if have only one investment easy to return the CAR.
            if (values.Count == 1)
            {
                return FinanceFunctions.CAR(latestValue, FirstValuation(values));
            }

            return FinanceFunctions.IRR(values, latestValue);
        }

        /// <summary>
        /// Returns the internal rate of return between <param name="startValue"/> and <param name="latestValue"/>.
        /// </summary>
        internal double IRR(DailyNumeric startValue, DailyNumeric latestValue) => IRR(Values(), startValue, latestValue);

        private static double IRR(List<DailyNumeric> values, DailyNumeric startValue, DailyNumeric latestValue)
        {
            if (startValue == null || latestValue == null)
            {
                return double.NaN;
            }

            // if have only one investment easy to return the CAR.
            if (values.Count == 1)
            {
                return FinanceFunctions.CAR(latestValue, startValue);
            }

            return FinanceFunctions.IRR(startValue, GetValuesBetween(values, startValue.Day, latestValue.Day), latestValue);
        }
    }
}
