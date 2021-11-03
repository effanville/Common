using System;
using System.Collections.Generic;

using Common.Structure.FinanceFunctions;

namespace Common.Structure.DataStructures
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    public partial class TimeList
    {

        /// <inheritdoc />
        public TimeList Inverted()
        {
            return Inverted(Values());
        }

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
                        inverted.SetData(value.Day, double.PositiveInfinity);
                    }
                }
            }

            return inverted;
        }

        /// <summary>
        /// Adds all values in the list.
        /// </summary>
        public double Sum()
        {
            return Sum(Values());
        }

        private static double Sum(List<DailyValuation> values)
        {
            if (values.Count > 0)
            {
                double sum = 0;
                foreach (DailyValuation val in values)
                {
                    sum += val.Value;
                }

                return sum;
            }

            return double.NaN;
        }

        /// <summary>
        /// returns the CAR of the timelist between the dates provided.
        /// </summary>
        public double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return CAR(Values(), earlierTime, laterTime);
        }

        private static double CAR(List<DailyValuation> values, DateTime earlierTime, DateTime laterTime)
        {
            DailyValuation earlierValue = ValueOnOrBefore(values, earlierTime);
            DailyValuation laterValue = ValueOnOrBefore(values, laterTime);
            if (earlierValue == null || laterValue == null)
            {
                return double.NaN;
            }

            return FinancialFunctions.CAR(earlierValue, laterValue);
        }

        /// <summary>
        /// Returns internal rate of return of the values in the TimeList
        /// </summary>
        internal double IRR(DailyValuation latestValue)
        {
            return IRR(Values(), latestValue);
        }

        private static double IRR(List<DailyValuation> values, DailyValuation latestValue)
        {
            if (values == null || values.Count == 0 || latestValue == null)
            {
                return double.NaN;
            }

            // if have only one investment easy to return the CAR.
            if (values.Count == 1)
            {
                return FinancialFunctions.CAR(latestValue, FirstValuation(values));
            }

            return FinancialFunctions.IRR(values, latestValue);
        }

        /// <summary>
        /// Returns the internal rate of return between <param name="startValue"/> and <param name="latestValue"/>.
        /// </summary>
        internal double IRR(DailyValuation startValue, DailyValuation latestValue)
        {
            return IRR(Values(), startValue, latestValue);
        }

        private static double IRR(List<DailyValuation> values, DailyValuation startValue, DailyValuation latestValue)
        {
            if (startValue == null || latestValue == null)
            {
                return double.NaN;
            }

            // if have only one investment easy to return the CAR.
            if (values.Count == 1)
            {
                return FinancialFunctions.CAR(latestValue, startValue);
            }

            return FinancialFunctions.IRRTime(startValue, GetValuesBetween(values, startValue.Day, latestValue.Day), latestValue);
        }
    }
}
