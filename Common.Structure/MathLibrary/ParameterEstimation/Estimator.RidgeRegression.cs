using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.MathLibrary.Matrices;

namespace Common.Structure.MathLibrary.ParameterEstimation
{
    public static partial class Estimator
    {
        /// <summary>
        /// Estimator class for the Ridge regression weights.
        /// This calculates a solution of
        /// (X + lambda I)^(-1)X^Ty
        /// where lambda is some parameter
        /// either to be determined by the algorithm or pre specified.
        /// </summary>
        public static class RidgeRegression
        {
            public sealed class RidgeResult : Result
            {
                public double Lambda
                {
                    get;
                }

                public RidgeResult(
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

            public static RidgeResult Fit(double[,] fitData, double[] fitValues)
            {
                double bestLambda = CalculateBestLambda(fitData, fitValues);
                double[] estimator = CalculateRidgeWeights(fitData, fitValues, bestLambda);
                return new RidgeResult(
                    bestLambda,
                    fitData,
                    fitValues,
                    null,
                    estimator,
                    null,
                    double.NaN);
            }

            public static RidgeResult Fit(double[,] fitData, double[] fitValues, double lambda)
            {
                double[] estimator = CalculateRidgeWeights(fitData, fitValues, lambda);
                return new RidgeResult(
                    lambda,
                    fitData,
                    fitValues,
                    null,
                    estimator,
                    null,
                    double.NaN);
            }

            /// <summary>
            /// Calculates the Lasso weights from a given value of lambda. 
            /// </summary>
            /// <param name="data"></param>
            /// <param name="values">The values vector for each entry in data matrix.</param>
            /// <param name="lambda"></param>
            /// <returns></returns>
            private static double[] CalculateRidgeWeights(double[,] data, double[] values, double lambda)
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
            private static double CalculateBestLambda(double[,] data, double[] values, double lowerBound = 0.01, double upperBound = 0.11, double increment = 0.01, int numberPartitions = 5)
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
                    actualValues[i] = DoubleMatrix.VectorMatrixRowMult(data, weightsThisTime, partitionIndices[partitionIndex, i]);
                }

                return Residuals.MeanSquareError(actualValues, valuesSubset);
            }
        }
    }
}
