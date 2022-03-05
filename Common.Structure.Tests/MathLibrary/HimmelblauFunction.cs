using System;

using Common.Structure.MathLibrary.Optimisation.Vector;

namespace Common.Structure.Tests.MathLibrary.Optimisation.Vector
{
    /// <summary>
    /// f(x,y) = (x^2 + y - 11)^2 + (x + y^2 - 7)^2.
    /// Minima at (3.0,2.0), 
    /// (-2.805118,3.131312),
    /// (-3.779310,-3.283186),
    /// (3.584428,-1.848126)
    /// </summary>
    public sealed class HimmelblauFunction : IVectorFunction
    {
        /// <inheritdoc/>
        public VectorEvaluationPoint GlobalMinimum => new VectorEvaluationPoint(new[] { 3.0, 2.0 }, 0.0);

        public VectorEvaluationPoint[] LocalMinima =>
            new[]
            {
                new VectorEvaluationPoint(new[] { 3.0, 2.0 }, 0.0),
                new VectorEvaluationPoint(new[] { -2.805118,3.131312 }, 0.0),
                new VectorEvaluationPoint(new[] { -3.779310,-3.283186 }, 0.0),
                new VectorEvaluationPoint(new[] { 3.584428,-1.848126 }, 0.0),
            };

        /// <inheritdoc/>
        public VectorEvaluationPoint GlobalMaximum => throw new NotImplementedException();

        /// <inheritdoc/>
        public double Value(double[] value)
        {
            var first = value[0] * value[0] + value[1] - 11;
            var second = value[0] + value[1] * value[1] - 7;
            return first * first + second * second;
        }

        /// <inheritdoc/>
        public double[] Gradient(double[] value)
        {
            double[] output = new double[2];
            var first = value[0] * value[0] + value[1] - 11;
            var second = value[0] + value[1] * value[1] - 7;
            var xGradFirst = 2 * value[0];
            var yGradFirst = 1;
            var xGradSecond = 1;
            var yGradSecond = 2 * value[1];
            output[0] = 2 * first * xGradFirst + 2 * second * xGradSecond;
            output[1] = 2 * first * yGradFirst + 2 * second * yGradSecond;

            return output;
        }

        /// <inheritdoc/>
        public double[,] Hessian(double[] input)
        {
            throw new NotImplementedException();
        }
    }
}
