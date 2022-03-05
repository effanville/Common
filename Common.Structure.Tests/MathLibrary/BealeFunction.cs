using System;

using Common.Structure.MathLibrary.Optimisation.Vector;

namespace Common.Structure.Tests.MathLibrary.Optimisation.Vector
{
    public sealed class BealeFunction : IVectorFunction
    {
        /// <inheritdoc/>
        public VectorEvaluationPoint GlobalMinimum => new VectorEvaluationPoint(new[] { 3, 0.5 }, 0.0);

        /// <inheritdoc/>
        public VectorEvaluationPoint GlobalMaximum => throw new NotImplementedException();

        /// <inheritdoc/>
        public double Value(double[] value)
        {
            var first = 1.5 - value[0] + value[0] * value[1];
            var second = 2.25 - value[0] + value[0] * Math.Pow(value[1], 2);
            var third = 2.625 - value[0] + value[0] * Math.Pow(value[1], 3);
            return first * first + second * second + third * third;
        }

        /// <inheritdoc/>
        public double[] Gradient(double[] value)
        {
            double[] output = new double[2];
            var first = 1.5 - value[0] + value[0] * value[1];
            var xGradFirst = -1 + value[1];
            var yGradFirst = value[0];
            var secondSquared = Math.Pow(value[1], 2);
            var second = 2.25 - value[0] + value[0] * secondSquared;
            var xGradSecond = -1 + secondSquared;
            var yGradSecond = 2 * value[0] * value[1];
            var secondParamCubed = Math.Pow(value[1], 3);
            var third = 2.625 - value[0] + value[0] * secondParamCubed;
            var xGradThird = -1 + secondParamCubed;
            var yGradThird = 3 * secondSquared * value[0];
            output[0] = 2 * first * xGradFirst + 2 * second * xGradSecond + 2 * third * xGradThird;
            output[1] = 2 * first * yGradFirst + 2 * second * yGradSecond + 2 * third * yGradThird;

            return output;
        }

        /// <inheritdoc/>
        public double[,] Hessian(double[] input)
        {
            throw new NotImplementedException();
        }
    }
}
