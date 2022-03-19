using System;

namespace Common.Structure.MathLibrary.Optimisation.Scalar
{
    /// <summary>
    /// Contains a scalar function point and its value.
    /// </summary>
    public struct ScalarFuncEvaluation
    {
        /// <summary>
        /// The point of the evaluation.
        /// </summary>
        public double Point
        {
            get;
        }

        /// <summary>
        /// The value at this point.
        /// </summary>
        public double Value
        {
            get;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public ScalarFuncEvaluation(double point, double value)
        {
            Point = point;
            Value = value;
        }

        /// <summary>
        /// Construct an instance from a point and a function.
        /// </summary>
        public ScalarFuncEvaluation(double point, Func<double, double> function)
        {
            Point = point;
            Value = function(point);
        }
    }
}
