using System;
using System.Collections.Generic;

using Common.Structure.MathLibrary.Finance;

namespace Common.Structure.DataStructures.Numeric
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    public partial class TimeNumberList
    {

        /// <inheritdoc />
        public TimeNumberList Inverted()
        {
            return Inverted(Values());
        }

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
        public double Sum()
        {
            return Sum(Values());
        }

        private static double Sum(List<DailyNumeric> values)
        {
            if (values.Count > 0)
            {
                double sum = 0;
                foreach (DailyNumeric val in values)
                {
                    sum += val.Value;
                }

                return sum;
            }

            return 0.0;
        }

        /// <summary>
        /// returns the CAR of the timelist between the dates provided.
        /// </summary>
        public double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return CAR(Values(), earlierTime, laterTime);
        }

        private static double CAR(List<DailyNumeric> values, DateTime earlierTime, DateTime laterTime)
        {
            DailyNumeric earlierValue = ValueOnOrBefore(values, earlierTime);
            DailyNumeric laterValue = ValueOnOrBefore(values, laterTime);
            if (earlierValue == null || laterValue == null)
            {
                return double.NaN;
            }

            return FinanceFunctions.CAR(earlierValue, laterValue);
        }

        /// <summary>
        /// Returns internal rate of return of the values in the TimeList
        /// </summary>
        internal double IRR(DailyNumeric latestValue)
        {
            return IRR(Values(), latestValue);
        }

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
        internal double IRR(DailyNumeric startValue, DailyNumeric latestValue)
        {
            return IRR(Values(), startValue, latestValue);
        }

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
