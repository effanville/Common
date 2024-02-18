using System;

using Effanville.Common.Structure.MathLibrary.Optimisation.Vector;

namespace Effanville.Common.Structure.Tests.MathLibrary
{
    public sealed class RosenbrockFunction : IVectorFunction
    {
        /// <inheritdoc/>
        public VectorFuncEval GlobalMinimum => new VectorFuncEval(new[] { 1.0, 1.0 }, 0.0);

        /// <inheritdoc/>
        public VectorFuncEval GlobalMaximum => throw new NotImplementedException();

        /// <inheritdoc/>
        public double Value(double[] value)
        {
            return Math.Pow((1 - value[0]), 2) + 100 * Math.Pow((value[1] - value[0] * value[0]), 2);
        }

        /// <inheritdoc/>
        public double[] Gradient(double[] value)
        {
            double[] output = new double[2];
            output[0] = -2 * (1 - value[0]) + 200 * (value[1] - value[0] * value[0]) * (-2 * value[0]);
            output[1] = 2 * 100 * (value[1] - value[0] * value[0]);
            return output;
        }

        /// <inheritdoc/>
        public double[,] Hessian(double[] input)
        {
            double[,] output = new double[2, 2];
            output[0, 0] = 2 - 400 * input[1] + 1200 * input[0] * input[0];
            output[1, 1] = 200;
            output[0, 1] = -400 * input[0];
            output[1, 0] = output[0, 1];
            return output;
        }
    }
}
