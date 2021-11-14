using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.MathLibrary.Matrices;

namespace Common.Structure.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Estimator class for the Ridge regression weights.
    /// This calculates a solution of
    /// (X + lambda I)^(-1)X^Ty
    /// where X = <see cref="FitData"/> and y = <see cref="FitValues"/> and lambda is some parameter
    /// either to be determined by the algorithm or pre specified.
    /// </summary>
    public sealed class RidgeRegression : IEstimator
    {
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

        /// <inheritdoc/>
        public double[] Estimator
        {
            get;
            private set;
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

        /// <summary>
        /// Generates an instance.
        /// </summary>
        /// <param name="data">
        /// <para>The data matrix to be used. </para>
        /// <para>Each row is a new observation, </para>
        /// <para>each column represents a different parameter to estimate.</para>
        /// </param>
        /// <param name="values">The outcome values for each observation.</param>
        public RidgeRegression(double[,] data, double[] values)
        {
            if (data.GetLength(0) != values.Length)
            {
                return;
            }

            FitData = data;
            FitValues = values;

            GenerateEstimator(data, values);
        }

        /// <summary>
        /// Generates an instance.
        /// </summary>
        /// <param name="data">
        /// <para>The data matrix to be used. </para>
        /// <para>Each row is a new observation, </para>
        /// <para>each column represents a different parameter to estimate.</para>
        /// </param>
        /// <param name="values">The outcome values for each observation.</param>
        /// <param name="lambda">Pre specified value of lambda to use in the LASSO algorithm.</param>
        public RidgeRegression(double[,] data, double[] values, double lambda)
        {
            if (data.GetLength(0) != values.Length)
            {
                return;
            }

            FitData = data;
            FitValues = values;
            Estimator = CalculateRidgeWeights(data, values, lambda);
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
            double bestlambda = CalculateBestLambda(data, values);
            Estimator = CalculateRidgeWeights(data, values, bestlambda);
        }

        /// <inheritdoc/>
        public void GenerateEstimator(double[,] data, double[] values, double[] sigmaValues)
        {
            GenerateEstimator(data, values, sigmaValues);
        }

        /// <summary>
        /// Calculates the Lasso weights from a given value of lambda. 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values">The values vector for each entry in data matrix.</param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        private double[] CalculateRidgeWeights(double[,] data, double[] values, double lambda)
        {
            // compute M = (data^T data+ lambda^2 I)^-1
            double[,] datamod = data.XTXPlusI(Math.Pow(lambda, 2));
            double[,] inverse = datamod.Inverse();
            double[] transpose = data.Transpose().PostMultiplyVector(values);
            double[] matrix = inverse.PostMultiplyVector(transpose);
            return matrix;
        }

        /// <summary>
        /// Mechanism to provide the optimal lambda parameter to use in a given range.
        /// </summary>
        /// <param name="data">The data matrix to use.</param>
        /// <param name="values">The values vector for each entry in data matrix.</param>
        /// <param name="lowerBound">Lower bound for lambda.</param>
        /// <param name="upperBound">Upper bound for lambda.</param>
        /// <param name="increment">Increment value to use in the range.</param>
        /// <param name="numberPartitions">The number of partitions to use in the estimation.</param>
        /// <returns></returns>
        private double CalculateBestLambda(double[,] data, double[] values, double lowerBound = 0.01, double upperBound = 0.11, double increment = 0.01, int numberPartitions = 5)
        {
            Dictionary<double, double> errorValues = new Dictionary<double, double>();
            int tot = data.GetLength(0) / numberPartitions;
            int[,] PartitionIndices = new int[5, tot];
            for (int i = 0; i < numberPartitions; i++)
            {
                for (int k = 0; k < tot; k++)
                {
                    PartitionIndices[i, k] = tot * i + k;
                }
            }
            double lambdaValue = lowerBound;
            while (lambdaValue <= upperBound)
            {
                double error = 0;
                for (int partitionIndex = 0; partitionIndex < PartitionIndices.GetLength(0); partitionIndex++)
                {
                    error += ErrorFromExpected(lambdaValue, PartitionIndices, partitionIndex, data, values);
                }
                errorValues.Add(lambdaValue, error);
                lambdaValue += increment;
            }

            KeyValuePair<double, double> smallestError = errorValues.First();
            foreach (KeyValuePair<double, double> errorValue in errorValues)
            {
                if (errorValue.Value < smallestError.Value)
                {
                    smallestError = errorValue;
                }
            }
            return smallestError.Key;
        }

        private double ErrorFromExpected(double lambda, int[,] partitionIndices, int partitionIndex, double[,] data, double[] values)
        {
            double[,] dataSubset = new double[partitionIndices.GetLength(1), data.GetLength(1)];
            double[] valuesSubset = new double[partitionIndices.GetLength(1)];

            for (int j = 0; j < partitionIndices.GetLength(1); j++)
            {
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    dataSubset[j, i] = data[partitionIndices[partitionIndex, j], i];
                }
                valuesSubset[j] = values[partitionIndices[partitionIndex, j]];
            }

            double[] weightsThisTime = CalculateRidgeWeights(dataSubset, valuesSubset, lambda);
            double[] actualValues = new double[partitionIndices.GetLength(1)];
            for (int i = 0; i < partitionIndices.GetLength(1); i++)
            {
                actualValues[i] = MatrixFunctions.VectorMatrixRowMult(data, weightsThisTime, partitionIndices[partitionIndex, i]);
            }

            return Residuals.MeanSquareError(actualValues, valuesSubset);
        }
    }
}
