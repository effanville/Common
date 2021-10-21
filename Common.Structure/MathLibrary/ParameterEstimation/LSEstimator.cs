﻿using Common.Structure.MathLibrary.Matrices;

namespace Common.Structure.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Holds data on least squares estimator for a matrix of data inputs and corresponding y values.
    /// </summary>
    public class LSEstimator : IEstimator
    {
        /// <inheritdoc/>
        public double[] Estimator
        {
            get; private set;
        }

        /// <inheritdoc/>
        public double[,] Uncertainty
        {
            get; private set;
        }

        /// <inheritdoc/>
        public double GoodnessOfFit
        {
            get; private set;
        }

        /// <inheritdoc/>
        public int NumberOfParameters
        {
            get
            {
                return Estimator.Length;
            }
        }

        /// <inheritdoc/>
        public int NumberOfDataPoints
        {
            get
            {
                return FitValues.Length;
            }
        }

        /// <inheritdoc/>
        public double[,] FitData
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public double[] FitValues
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor. This calculates the estimator.
        /// </summary>
        /// <param name="data">The data matrix used in the estimation.</param>
        /// <param name="values">The values matrix used in the estimation.</param>
        public LSEstimator(double[,] data, double[] values)
        {
            GenerateEstimator(data, values);
        }

        /// <inheritdoc/>
        public double Evaluate(double[] point)
        {
            if (Estimator.Length != point.Length)
            {
                return double.NaN;
            }
            double value = 0.0;
            for (int index = 0; index < Estimator.Length; index++)
            {
                value += Estimator[index] * point[index];
            }

            return value;
        }

        /// <inheritdoc/>
        public void GenerateEstimator(double[,] data, double[] values)
        {
            FitData = data;
            FitValues = values;
            double[] XTY = data.Transpose().PostMultiplyVector(values);
            var matrix = data.XTX().Inverse();
            Estimator = matrix.PostMultiplyVector(XTY);
            var residual = 0.0;
            for (int i = 0; i < FitData.GetLength(0); i++)
            {
                double value = FitValues[i];
                for (int j = 0; j < FitData.GetLength(1); j++)
                {
                    value -= Estimator[j] * FitData[i, j];
                }

                residual += value * value;
            }
            residual /= (NumberOfDataPoints - NumberOfParameters);
            Uncertainty = matrix.ScalarMult(residual);
        }
    }
}
