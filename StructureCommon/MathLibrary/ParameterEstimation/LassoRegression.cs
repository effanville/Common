using System;

namespace StructureCommon.MathLibrary.ParameterEstimation
{
    public sealed class LassoRegression : IEstimator
    {
        /// <inheritdoc/>
        public int NumberOfParameters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public int NumberOfDataPoints
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public double[,] FitData
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public double[] FitValues
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public double[] Estimator
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public double[,] Uncertainty
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public double GoodnessOfFit
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public double Evaluate(double[] point)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void GenerateEstimator(double[,] data, double[] values)
        {
            throw new NotImplementedException();
        }
    }
}
