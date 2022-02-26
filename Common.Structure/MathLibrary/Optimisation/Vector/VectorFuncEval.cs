using System;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    /// <summary>
    /// Contains function data for a function from R^n to R.
    /// </summary>
    public struct VectorFuncEval
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
        public VectorFuncEval(double[] point, double value)
        {
            Point = point;
            Value = value;
        }

        /// <summary>
        /// Construct an instance from a point and a function.
        /// </summary>
        public VectorFuncEval(double[] point, Func<double[], double> function)
        {
            Point = point;
            Value = function(point);
        }
    }
}
