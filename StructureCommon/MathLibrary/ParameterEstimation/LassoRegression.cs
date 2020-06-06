using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureCommon.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Estimator class for the Lasso weights.
    /// </summary>
    public sealed class LassoRegression : IEstimator
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
            double[,] matrixDataValues = new double[data.GetLength(0), data.GetLength(1) + 1];
            for (int i = 0; i < matrixDataValues.GetLength(0); i++)
            {
                for (int j = 0; j < matrixDataValues.GetLength(1); j++)
                {
                    if (j == matrixDataValues.GetLength(1))
                    {
                        matrixDataValues[i, j] = values[i];
                    }
                    else
                    {
                        matrixDataValues[i, j] = data[i, j];
                    }
                }
            }

            double bestlambda = CalculateBestLambda(matrixDataValues.GetLength(0), matrixDataValues);

            Estimator = CalculateLassoWeights(data.GetLength(1), matrixDataValues.GetLength(0), matrixDataValues, bestlambda);
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
        public LassoRegression(double[,] data, double[] values)
        {
            if (data.GetLength(0) != values.Length)
            {
                return;
            }

            FitData = data;
            FitValues = values;
            NumberOfDataPoints;
            NumberOfParameters= data.

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
        public LassoRegression(double[,] data, double[] values, double lambda)
        {
            if (data.GetLength(0) != values.Length)
            {
                return;
            }

            double[,] matrixDataValues = new double[data.GetLength(0), data.GetLength(1) + 1];
            for (int i = 0; i < matrixDataValues.GetLength(0); i++)
            {
                for (int j = 0; j < matrixDataValues.GetLength(1); j++)
                {
                    if (j == matrixDataValues.GetLength(1))
                    {
                        matrixDataValues[i, j] = values[i];
                    }
                    else
                    {
                        matrixDataValues[i, j] = data[i, j];
                    }
                }
            }

            FitData = data;
            FitValues = values;
            Estimator = CalculateLassoWeights(data.GetLength(1), matrixDataValues.GetLength(0), matrixDataValues, lambda);
        }

        /*Computes the normalization factor*/
        private double NormalisationFactor(int columnIndex, double[,] matrix, int numberMatrixRows)
        {
            double sum = 0.0;
            for (int rowIndex = 0; rowIndex < numberMatrixRows; rowIndex++)
            {
                sum += Math.Pow(matrix[rowIndex, columnIndex], 2);
            }

            return sum;
        }

        /*Computes the partial sum*/
        double PartialSum(int numberDimensions, int ignoreDimension, double[] weights, double[,] matrix, int rowIndex)
        {
            double sum = 0.0;
            for (int j = 0; j < numberDimensions; j++)
            {
                if (j != ignoreDimension)
                {
                    sum += weights[j] * matrix[rowIndex, j];
                }
            }

            return sum;
        }

        /* Computes rho-j */
        double Rho_j(double[,] arr, int n, int j, int num_dim, double[] weights)
        {
            double sum = 0.0;
            double partial_sum;
            for (int rowIndex = 0; rowIndex < n; rowIndex++)
            {
                partial_sum = PartialSum(num_dim, j, weights, arr, rowIndex);
                sum += arr[rowIndex, j] * (arr[rowIndex, num_dim] - partial_sum);
            }

            return sum;
        }

        double Intercept(double[] weights, double[,] arr, int num_dim, int num_obs)
        {
            double sum = 0.0;
            for (int rowIndex = 0; rowIndex < num_obs; rowIndex++)
            {
                sum += Math.Pow((arr[rowIndex, num_dim]) - PartialSum(num_dim, -1, weights, arr, rowIndex), 1);
            }

            return sum;
        }

        /// <summary>
        /// Calculates the Lasso weights from a given value of lambda. 
        /// </summary>
        /// <param name="numberParameters"></param>
        /// <param name="numberObservations"></param>
        /// <param name="data"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        double[] CalculateLassoWeights(int numberParameters, int numberObservations, double[,] data, double lambda)
        {
            double a = 1.0;
            // Instantiate the weights vector, including an intercept parameter.
            double[] weights = new double[numberParameters + 1];
            double[] OldWeights;

            int i;

            //Initializing the weight vector
            for (i = 0; i < numberParameters; i++)
            {
                weights[i] = a;
            }

            int rowIndex = 0;
            double[] rho = new double[numberParameters];
            for (i = 0; i < numberParameters; i++)
            {
                rho[i] = Rho_j(data, numberObservations, rowIndex, numberParameters, weights);
            }

            // Intercept initialization
            weights[numberParameters] = Intercept(weights, data, numberParameters, numberObservations);

            int maxIterations = 500;
            int interation = 0;
            while (interation < maxIterations)
            {
                OldWeights = weights;
                for (rowIndex = 0; rowIndex < numberParameters; rowIndex++)
                {
                    rho[rowIndex] = Rho_j(data, numberObservations, rowIndex, numberParameters, weights);
                    if (rho[rowIndex] < -lambda / 2)
                    {
                        weights[rowIndex] = (rho[rowIndex] + lambda / 2) / NormalisationFactor(rowIndex, data, numberObservations);
                    }
                    else if (rho[rowIndex] > lambda / 2)
                    {
                        weights[rowIndex] = (rho[rowIndex] - lambda / 2) / NormalisationFactor(rowIndex, data, numberObservations);
                    }
                    else
                    {
                        weights[rowIndex] = 0;
                    }

                    weights[numberParameters] = Intercept(weights, data, numberParameters, numberObservations);
                }
                if (LassoStatsCalculation.MeanSquareError(OldWeights, weights) < 5e-3)
                {
                    break;
                }

                interation++;
            }

            return weights;
        }

        /// <summary>
        /// Mechanism to provide the optimal lambda parameter to use in a given range.
        /// </summary>
        /// <param name="numberObservations"></param>
        /// <param name="data">The data matrix to use.</param>
        /// <param name="lowerBound">Lower bound for lambda.</param>
        /// <param name="upperBound">Upper bound for lambda.</param>
        /// <param name="increment">Increment value to use in the range.</param>
        /// <param name="numberPartitions">The number of partitions to use in the estimation.</param>
        /// <returns></returns>
        double CalculateBestLambda(int numberObservations, double[,] data, double lowerBound = 0.1, double upperBound = 1, double increment = 0.1, int numberPartitions = 5)
        {
            Dictionary<double, double> errorValues = new Dictionary<double, double>();
            int tot = numberObservations / numberPartitions;
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
                    error += ErrorFromExpected(lambdaValue, PartitionIndices, partitionIndex, data);
                }
                errorValues.Add(lambdaValue, error);
                lambdaValue += increment;
            }

            return errorValues.Max().Key;
        }

        double ErrorFromExpected(double lambda, int[,] partitionIndices, int partitionIndex, double[,] data)
        {
            double[,] dataSubset = new double[partitionIndices.GetLength(1), data.GetLength(1)];
            for (int i = 0; i < data.GetLength(1); i++)
            {
                for (int j = 0; j < partitionIndices.GetLength(1); j++)
                {
                    dataSubset[i, j] = data[partitionIndices[partitionIndex, j], i];
                }
            }

            double[] weightsThisTime = CalculateLassoWeights(dataSubset.GetLength(1), dataSubset.GetLength(0), dataSubset, lambda);
            double[] actualValues = new double[partitionIndices.GetLength(1)];
            double[] expectedValues = new double[partitionIndices.GetLength(1)];
            for (int i = 0; i < partitionIndices.GetLength(1); i++)
            {
                expectedValues[i] = data[partitionIndices[partitionIndex, i], data.GetLength(1) - 1];
                actualValues[i] = LassoStatsCalculation.VectorMatrixRowMult(data, weightsThisTime, partitionIndices[partitionIndex, i]);
            }

            return LassoStatsCalculation.MeanSquareError(actualValues, expectedValues);
        }
    }
}
