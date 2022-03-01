using System;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    /// <summary>
    /// Contains function data for a function from R^n to R.
    /// </summary>
    public sealed class VectorEvaluationPoint
    {
        /// <summary>
        /// The point of the evaluation.
        /// </summary>
        public double[] Point
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
        public VectorEvaluationPoint(double[] point, double value)
        {
            Point = point;
            Value = value;
        }

        /// <summary>
        /// Construct an instance from a point and a function.
        /// </summary>
        public VectorEvaluationPoint(double[] point, Func<double[], double> function)
        {
            Point = point;
            Value = function(point);
        }
    }
}
