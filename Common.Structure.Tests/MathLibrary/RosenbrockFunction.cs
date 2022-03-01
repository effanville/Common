using System;
using System.Linq;

namespace Common.Structure.Tests.MathLibrary.Optimisation.Vector
{
    public sealed class BigRosenbrockFunction : IVectorFunction
    {
        private RosenbrockFunction fRosenbrockFunction = new RosenbrockFunction();

        public double[] GlobalMinimum => new double[] { 100, 100 };

        public double[] GlobalMaximum => throw new NotImplementedException();

        public double Value(double[] value)
        {
            double[] scaledValue = value.Select(val => val / 100.0).ToArray();
            return 1000.0 + 100.0 * fRosenbrockFunction.Value(scaledValue);
        }

        public double[] Gradient(double[] value)
        {
            double[] scaledValue = value.Select(val => val / 100.0).ToArray();
            return fRosenbrockFunction.Gradient(scaledValue).Select(val => val * 100.0).ToArray();
        }

        /// <inheritdoc/>
        public double[,] Hessian(double[] value)
        {
            double[] scaledValue = value.Select(val => val / 100.0).ToArray();
            var hessian = fRosenbrockFunction.Hessian(scaledValue);
            double[,] output = new double[2, 2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    output[i, j] = 100.0 * hessian[i, j];
                }
            }

            return output;
        }
    }

    public sealed class RosenbrockFunction : IVectorFunction
    {
        /// <inheritdoc/>
        public double[] GlobalMinimum => new double[] { 1, 1 };

        /// <inheritdoc/>
        public double[] GlobalMaximum => throw new NotImplementedException();

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
