using System;
using System.Collections.Generic;
using System.Linq;
using Common.Structure.MathLibrary.Matrices;

namespace Common.Structure.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// <para>Estimator class for the Lasso weights. This calculates the weights w 
    /// such that the functional</para>
    /// <para>(2n)^-1 || y-Xw||^2 + lambda||w||_1</para>
    /// <para>is minimised, where y = <see cref="FitValues"/> and X = <see cref="FitData"/>.</para>
    /// <para>The method used here is the coordinate descent method, which proceeds as follows.</para>
    /// <para>Given an approximate solution w, one calculates the partial residuals</para>
    /// <para>r_ij = y_i- sum_(-j) x_ik w_k and z_j = n^-1 sum x_ij r_ij</para>    
    /// <para>For each j, one then solves the 1 dimensional lasso problem to obtain the next weights value.</para>    
    /// <para>This terminates when number of iterations is too high, or the residual error is small enough.</para>
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
            double bestlambda = CalculateBestLambda(data, values);

            Estimator = CalculateLassoWeights(data, values, bestlambda);
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

            FitData = data;
            FitValues = values;
            Estimator = CalculateLassoWeights(data, values, lambda);
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
        private double PartialSum(int numberParameterDimensions, int ignoreParameterIndex, double[] weights, double[,] matrix, int rowIndex)
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
        private double Rho_j(double[,] matrix, double[] values, int numberObservationDimensions, int ignoreParameterIndex, int numberParameterDimensions, double[] weights)
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

        private double Intercept(double[] weights, double[,] arr, double[] values, int num_dim, int num_obs)
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
        /// <param name="data"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        private double[] CalculateLassoWeights(double[,] data, double[] values, double lambda, double tolerance = 1e-12, bool addIntercept = false)
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
        /// <param name="numberObservations"></param>
        /// <param name="data">The data matrix to use.</param>
        /// <param name="lowerBound">Lower bound for lambda.</param>
        /// <param name="upperBound">Upper bound for lambda.</param>
        /// <param name="increment">Increment value to use in the range.</param>
        /// <param name="numberPartitions">The number of partitions to use in the estimation.</param>
        /// <returns></returns>
        private double CalculateBestLambda(double[,] data, double[] values, double lowerBound = 0.1, double upperBound = 1, double increment = 0.1, int numberPartitions = 5)
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
                actualValues[i] = MatrixFunctions.VectorMatrixRowMult(data, weightsThisTime, partitionIndices[partitionIndex, i]);
            }

            return Residuals.MeanSquareError(actualValues, expectedValues);
        }
    }
}
