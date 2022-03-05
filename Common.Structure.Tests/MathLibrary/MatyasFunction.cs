using System;

using Common.Structure.MathLibrary.Optimisation.Vector;

namespace Common.Structure.Tests.MathLibrary.Optimisation.Vector
{
    /// <summary>
    /// f(x,y) = 0.26(x^2 + y^2) -0.48 x y
    /// </summary>
    public sealed class MatyasFunction : IVectorFunction
    {
        /// <inheritdoc/>
        public VectorEvaluationPoint GlobalMinimum => new VectorEvaluationPoint(new[] { 0.0, 0.0 }, 0.0);

        /// <inheritdoc/>
        public VectorEvaluationPoint GlobalMaximum => throw new NotImplementedException();

        /// <inheritdoc/>
        public double Value(double[] value)
        {
            return 0.26 * value[0] * value[0] + 0.26 * value[1] * value[1] - 0.48 * value[0] * value[1];
        }

        /// <inheritdoc/>
        public double[] Gradient(double[] value)
        {
            double[] output = new double[2];
            output[0] = 0.52 * value[0] - 0.48 * value[1];
            output[1] = 0.52 * value[1] - 0.48 * value[0];
            return output;
        }

        /// <inheritdoc/>
        public double[,] Hessian(double[] input)
        {
            throw new NotImplementedException();
        }
    }
}
