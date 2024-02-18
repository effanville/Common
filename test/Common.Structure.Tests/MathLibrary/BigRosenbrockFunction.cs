using System;
using System.Linq;

using Effanville.Common.Structure.MathLibrary.Optimisation.Vector;

namespace Effanville.Common.Structure.Tests.MathLibrary
{
    public sealed class BigRosenbrockFunction : IVectorFunction
    {
        private RosenbrockFunction fRosenbrockFunction = new RosenbrockFunction();

        public VectorFuncEval GlobalMinimum => new VectorFuncEval(new[] { 100.0, 100.0 }, 1000.0);

        public VectorFuncEval GlobalMaximum => throw new NotImplementedException();

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
}
