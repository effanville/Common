using System.Collections.Generic;
using System.Linq;

using Common.Structure.DataStructures;
using Common.Structure.DataStructures.Numeric;

namespace Common.Structure.MathLibrary.Finance
{
    /// <summary>
    /// Finance functions.
    /// </summary>
    public static partial class FinanceFunctions
    {
        /// <summary>
        /// Calculate the Maximum draw down  of a list of values.
        /// </summary>
        /// <param name="values">The list of values to calculate the mdd for.</param>
        /// <returns>A double value representing the MDD.</returns>
        public static decimal MDD(List<DailyValuation> values)
        {
            if (values != null && values.Any())
            {
                decimal maximumDrawDown = 0.0m;
                decimal peakValue = decimal.MinValue;
                for (int i = 0; i < values.Count; i++)
                {
                    decimal value = values[i].Value;
                    if (value > peakValue)
                    {
                        peakValue = value;
                    }

                    decimal drawDown = peakValue.Equals(0.0m) ? 0.0m : 100.0m * (peakValue - value) / peakValue;
                    if (drawDown > maximumDrawDown)
                    {
                        maximumDrawDown = drawDown;
                    }
                }

                return maximumDrawDown;
            }

            return decimal.MaxValue;
        }

        /// <summary>
        /// Calculate the Maximum draw down of a list of values.
        /// </summary>
        /// <param name="values">The list of values to calculate the mdd for.</param>
        /// <returns>A double value representing the MDD.</returns>
        public static double MDD(List<DailyNumeric> values)
        {
            if (values != null && values.Any())
            {
                double maximumDrawDown = 0.0;
                double peakValue = double.MinValue;
                for (int i = 0; i < values.Count; i++)
                {
                    double value = values[i].Value;
                    if (value > peakValue)
                    {
                        peakValue = value;
                    }

                    double drawDown = 100.0 * (peakValue - value) / peakValue;
                    if (drawDown > maximumDrawDown)
                    {
                        maximumDrawDown = drawDown;
                    }
                }

                return maximumDrawDown;
            }

            return double.NaN;
        }
    }
}
