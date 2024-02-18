using System.Collections.Generic;
using System.Linq;

using Common.Structure.DataStructures;
using Common.Structure.DataStructures.Numeric;

namespace Common.Structure.MathLibrary.Finance
{
    public static partial class FinanceFunctions
    {
        /// <summary>
        /// Calculates the DrawDown of a list of values.
        /// </summary>
        public static decimal Drawdown(List<DailyValuation> values)
        {
            if (values != null && values.Any())
            {
                decimal peakValue = decimal.MinValue;
                for (int i = 0; i < values.Count; i++)
                {
                    decimal value = values[i].Value;
                    if (value > peakValue)
                    {
                        peakValue = value;
                    }
                }

                return peakValue.Equals(0.0m) ? 0.0m : 100.0m * (peakValue - values.Last().Value) / peakValue;
            }

            return decimal.MaxValue;
        }


        /// <summary>
        /// Calculates the DrawDown of a list of values.
        /// </summary>
        public static double Drawdown(List<DailyNumeric> values)
        {
            if (values != null && values.Any())
            {
                double peakValue = double.MinValue;
                for (int i = 0; i < values.Count; i++)
                {
                    double value = values[i].Value;
                    if (value > peakValue)
                    {
                        peakValue = value;
                    }
                }

                return peakValue.Equals(0.0) ? 0.0 : 100.0 * (peakValue - values.Last().Value) / peakValue;
            }

            return double.NaN;
        }
    }
}
