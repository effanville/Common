using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.MathLibrary.Finance;

namespace Effanville.Common.Structure.DataStructures
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    public partial class TimeList
    {
        /// <inheritdoc />
        public TimeList Inverted() => Inverted(Values());

        private static TimeList Inverted(List<DailyValuation> values)
        {
            TimeList inverted = new TimeList();
            if (values.Count > 0)
            {
                foreach (DailyValuation value in values)
                {
                    if (!value.Value.Equals(0))
                    {
                        inverted.SetData(value.Day, 1 / value.Value);
                    }
                    else
                    {
                        inverted.SetData(value.Day, decimal.MaxValue);
                    }
                }
            }

            return inverted;
        }

        /// <summary>
        /// Adds all values in the list. The sum of an empty list is defined to be 0.
        /// </summary>
        public decimal Sum() => Sum(Values());

        /// <summary>
        /// Adds all values in the list that satisfy the predicate.
        /// </summary>
        public decimal Sum(Func<DailyValuation, bool> predicate)
            => Sum(Values().Where(val => predicate(val)));

        private static decimal Sum(IEnumerable<DailyValuation> values) => values.Sum(val => val.Value);

        /// <summary>
        /// returns the CAR of the timelist between the dates provided.
        /// </summary>
        public double CAR(DateTime earlierTime, DateTime laterTime) => CAR(Values(), earlierTime, laterTime);

        private static double CAR(List<DailyValuation> values, DateTime earlierTime, DateTime laterTime)
        {
            DailyValuation earlierValue = Value(values, earlierTime, OutlierInterpolation, OutlierInterpolation, DayBasedInterpolationFunction);
            DailyValuation laterValue = Value(values, laterTime, OutlierInterpolation, OutlierInterpolation, DayBasedInterpolationFunction);
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

        private static double CAROnOrBefore(List<DailyValuation> values, DateTime earlierTime, DateTime laterTime)
        {
            DailyValuation earlierValue = Value(values, earlierTime, (a, b) => null, OutlierInterpolation, (a, b, c) => a);
            DailyValuation laterValue = Value(values, laterTime, (a, b) => null, OutlierInterpolation, (a, b, c) => a); ;
            if (earlierValue == null || laterValue == null)
            {
                return double.NaN;
            }

            return FinanceFunctions.CAR(earlierValue, laterValue);
        }

        /// <summary>
        /// Returns internal rate of return of the values in the TimeList
        /// </summary>
        internal double IRR(DailyValuation latestValue) => IRR(Values(), latestValue);

        private static double IRR(List<DailyValuation> values, DailyValuation latestValue)
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
        internal double IRR(DailyValuation startValue, DailyValuation latestValue)
            => IRR(Values(), startValue, latestValue);

        private static double IRR(List<DailyValuation> values, DailyValuation startValue, DailyValuation latestValue)
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
