using System;
using System.Collections.Generic;
using System.Linq;
using StructureCommon.MathLibrary.Matrices;

namespace StructureCommon.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Estimator class for the Lasso weights.
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

        /// <summary>
        /// Calculates the Lasso weights from a given value of lambda. 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="values">The values vector for each entry in data matrix.</param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        double[] CalculateRidgeWeights(double[,] data, double[] values, double lambda)
        {
            double[,] datamod = data.XTXPlusI(Math.Pow(lambda, 2));


            // compute M = (data^T data+ lambda^2 I)^-1
            var decomp = new LUDecomposition(datamod);
            double[,] inverse = decomp.Inverse();

            return inverse.Multiply(data.Transpose()).PostMultiplyVector(values);
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
        double CalculateBestLambda(double[,] data, double[] values, double lowerBound = 0.01, double upperBound = 0.11, double increment = 0.01, int numberPartitions = 5)
        {
            Dictionary<double, double> errorValues = new Dictionary<double, double>();
            int tot = data.GetLength(1) / numberPartitions;
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
                for (int partitionIndex = 0; partitionIndex < PartitionIndices.GetLength(1); partitionIndex++)
                {
                    error += ErrorFromExpected(lambdaValue, PartitionIndices, partitionIndex, data, values);
                }
                errorValues.Add(lambdaValue, error);
                lambdaValue += increment;
            }

            return errorValues.Max().Key;
        }

        double ErrorFromExpected(double lambda, int[,] partitionIndices, int partitionIndex, double[,] data, double[] values)
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
            double[] expectedValues = new double[partitionIndices.GetLength(1)];
            for (int i = 0; i < partitionIndices.GetLength(1); i++)
            {
                expectedValues[i] = values[partitionIndices[partitionIndex, i]];
                actualValues[i] = LassoStatsCalculation.VectorMatrixRowMult(data, weightsThisTime, partitionIndices[partitionIndex, i]);
            }

            return LassoStatsCalculation.MeanSquareError(actualValues, expectedValues);
        }
    }
}
