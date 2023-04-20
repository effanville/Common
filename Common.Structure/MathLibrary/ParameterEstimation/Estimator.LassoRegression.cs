using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.MathLibrary.Matrices;

namespace Common.Structure.MathLibrary.ParameterEstimation
{
    public static partial class Estimator
    {
        /// <summary>
        /// <para>Estimator class for the Lasso weights. This calculates the weights w 
        /// such that the functional</para>
        /// <para>(2n)^-1 || y-Xw||^2 + lambda||w||_1</para>
        /// <para>is minimised.</para>
        /// <para>The method used here is the coordinate descent method, which proceeds as follows.</para>
        /// <para>Given an approximate solution w, one calculates the partial residuals</para>
        /// <para>r_ij = y_i- sum_(-j) x_ik w_k and z_j = n^-1 sum x_ij r_ij</para>    
        /// <para>For each j, one then solves the 1 dimensional lasso problem to obtain the next weights value.</para>    
        /// <para>This terminates when number of iterations is too high, or the residual error is small enough.</para>
        /// </summary>
        public static class LassoRegression
        {
            /// <summary>
            /// A result object containing the result of a <see cref="LassoRegression"/>.
            /// </summary>
            public sealed class LassoResult : Result
            {
                /// <summary>
                /// The value of Lambda to use.
                /// </summary>
                public double Lambda
                {
                    get;
                }

                /// <summary>
                /// Construct an instance of a <see cref="LassoResult"/>
                /// </summary>
                public LassoResult(
                    double lambda,
                    double[,] fitData,
                    double[] fitValues,
                    double[] sigma,
                    double[] estimator,
                    double[,] estimatorCovariance,
                    double estimatorFit)
                    : base(fitData,
                          fitValues,
                          sigma,
                          estimator,
                          estimatorCovariance,
                          estimatorFit)
                {
                    Lambda = lambda;
                }

                /// <inheritdoc/>
                public override double Evaluate(double[] point)
                {
                    return EstimatorHelpers.Evaluate(Estimator, point);
                }
            }

            /// <summary>
            /// Fit the data to the values and find the best lambda value to use.
            /// </summary>
            public static LassoResult Fit(double[,] fitData, double[] fitValues)
            {
                double bestLambda = CalculateBestLambda(fitData, fitValues);

                double[] estimator = CalculateLassoWeights(fitData, fitValues, bestLambda);
                return new LassoResult(
                    bestLambda,
                    fitData,
                    fitValues,
                    null,
                    estimator,
                    null,
                    double.NaN);
            }

            /// <summary>
            /// Fit the data to the values with the given lambda.
            /// </summary>
            public static LassoResult Fit(double[,] fitData, double[] fitValues, double lambda)
            {
                double[] estimator = CalculateLassoWeights(fitData, fitValues, lambda);
                return new LassoResult(
                    lambda,
                    fitData,
                    fitValues,
                    null,
                    estimator,
                    null,
                    double.NaN);
            }

            /*Computes the normalization factor*/
            private static double NormalisationFactor(int columnIndex, double[,] matrix, int numberMatrixRows)
            {
                double sum = 0.0;
                for (int rowIndex = 0; rowIndex < numberMatrixRows; rowIndex++)
                {
                    sum += Math.Pow(matrix[rowIndex, columnIndex], 2);
                }

                return sum;
            }

            /*Computes the partial sum*/
            private static double PartialSum(int numberParameterDimensions, int ignoreParameterIndex, double[] weights, double[,] matrix, int rowIndex)
            {
                double sum = 0.0;
                for (int j = 0; j < numberParameterDimensions; j++)
                {
                    if (j != ignoreParameterIndex)
                    {
                        sum += weights[j] * matrix[rowIndex, j];
                    }
                }

                return sum;
            }

            /* Computes rho-j */
            private static double Rho_j(double[,] matrix, double[] values, int numberObservationDimensions, int ignoreParameterIndex, int numberParameterDimensions, double[] weights)
            {
                double sum = 0.0;
                double partial_sum;
                for (int rowIndex = 0; rowIndex < numberObservationDimensions; rowIndex++)
                {
                    partial_sum = PartialSum(numberParameterDimensions, ignoreParameterIndex, weights, matrix, rowIndex);
                    sum += matrix[rowIndex, ignoreParameterIndex] * (values[rowIndex] - partial_sum);
                }

                return sum;
            }

            private static double Intercept(double[] weights, double[,] arr, double[] values, int num_dim, int num_obs)
            {
                double sum = 0.0;
                for (int rowIndex = 0; rowIndex < num_obs; rowIndex++)
                {
                    sum += Math.Pow((values[rowIndex]) - PartialSum(num_dim, -1, weights, arr, rowIndex), 1);
                }

                return sum;
            }
            /// <summary>
            /// Calculates the Lasso weights from a given value of lambda. 
            /// </summary>
            private static double[] CalculateLassoWeights(double[,] data, double[] values, double lambda, double tolerance = 1e-12, bool addIntercept = false)
            {
                int numberParameters = data.GetLength(1);
                int numberObservations = data.GetLength(0);
                double a = 1.0;

                double[] weights;
                double[] OldWeights;
                // Instantiate the weights vector, including an intercept parameter.
                if (addIntercept)
                {
                    weights = new double[numberParameters + 1];
                    OldWeights = new double[numberParameters + 1];
                }
                else
                {
                    weights = new double[numberParameters];
                    OldWeights = new double[numberParameters];
                }


                int i;

                //Initializing the weight vector
                for (i = 0; i < numberParameters; i++)
                {
                    weights[i] = a;
                }

                double[] rho = new double[numberParameters];
                for (int columnParameterIndex = 0; columnParameterIndex < numberParameters; columnParameterIndex++)
                {
                    rho[columnParameterIndex] = Rho_j(data, values, numberObservations, columnParameterIndex, numberParameters, weights);
                }

                // Intercept initialization
                if (addIntercept)
                {
                    weights[numberParameters] = Intercept(weights, data, values, numberParameters, numberObservations);
                }

                int maxIterations = 1000;
                int iteration = 0;
                while (iteration < maxIterations)
                {
                    weights.CopyTo(OldWeights, 0);
                    for (int columnParameterIndex = 0; columnParameterIndex < numberParameters; columnParameterIndex++)
                    {
                        rho[columnParameterIndex] = Rho_j(data, values, numberObservations, columnParameterIndex, numberParameters, weights);
                        if (rho[columnParameterIndex] < -lambda / 2)
                        {
                            weights[columnParameterIndex] = (rho[columnParameterIndex] + lambda / 2) / NormalisationFactor(columnParameterIndex, data, numberObservations);
                        }
                        else if (rho[columnParameterIndex] > lambda / 2)
                        {
                            weights[columnParameterIndex] = (rho[columnParameterIndex] - lambda / 2) / NormalisationFactor(columnParameterIndex, data, numberObservations);
                        }
                        else
                        {
                            weights[columnParameterIndex] = 0;
                        }

                        if (addIntercept)
                        {
                            weights[numberParameters] = Intercept(weights, data, values, numberParameters, numberObservations);
                        }
                    }

                    double error = Residuals.MeanSquareError(OldWeights, weights);
                    if (error < tolerance)
                    {
                        break;
                    }

                    iteration++;
                }

                return weights;
            }

            /// <summary>
            /// Mechanism to provide the optimal lambda parameter to use in a given range.
            /// </summary>
            /// <param name="data">The data matrix to use.</param>
            /// <param name="values">The expected value matrix to use.</param>
            /// <param name="lowerBound">Lower bound for lambda.</param>
            /// <param name="upperBound">Upper bound for lambda.</param>
            /// <param name="increment">Increment value to use in the range.</param>
            /// <param name="numberPartitions">The number of partitions to use in the estimation.</param>
            /// <returns></returns>
            private static double CalculateBestLambda(
                double[,] data,
                double[] values,
                double lowerBound = 0.1,
                double upperBound = 1,
                double increment = 0.1,
                int numberPartitions = 5)
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

            private static double ErrorFromExpected(double lambda, int[,] partitionIndices, int partitionIndex, double[,] data, double[] values)
            {
                double[,] dataSubset = new double[partitionIndices.GetLength(1), data.GetLength(1)];
                double[] valuesSubset = new double[partitionIndices.GetLength(1)];
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    for (int j = 0; j < partitionIndices.GetLength(1); j++)
                    {
                        dataSubset[j, i] = data[partitionIndices[partitionIndex, j], i];
                    }
                }

                for (int j = 0; j < partitionIndices.GetLength(1); j++)
                {
                    valuesSubset[j] = values[partitionIndices[partitionIndex, j]];
                }

                double[] weightsThisTime = CalculateLassoWeights(dataSubset, valuesSubset, lambda);
                double[] actualValues = new double[partitionIndices.GetLength(1)];
                double[] expectedValues = new double[partitionIndices.GetLength(1)];
                for (int i = 0; i < partitionIndices.GetLength(1); i++)
                {
                    expectedValues[i] = data[partitionIndices[partitionIndex, i], data.GetLength(1) - 1];
                    actualValues[i] = DoubleMatrix.VectorMatrixRowMult(data, weightsThisTime, partitionIndices[partitionIndex, i]);
                }

                return Residuals.MeanSquareError(actualValues, expectedValues);
            }
        }
    }
}
