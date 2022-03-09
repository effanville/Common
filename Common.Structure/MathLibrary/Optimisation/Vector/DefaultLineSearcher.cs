﻿using System;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    public sealed class DefaultLineSearcher : ILineSearcher
    {
        private double MaxStepLength;
        public DefaultLineSearcher(double maxStepLength)
        {
            MaxStepLength = maxStepLength;
        }

        /// <inheritdoc/>
        public Result<VectorEvaluationPoint> FindConformingStep(
            double[] startingPoint,
            double startingValue,
            double[] startingDerivative,
            double[] searchDirection,
            Func<double[], double> func,
            int maxIterations = 100)
        {
            int numDimensions = startingPoint.Length;
            double[] candidateOutputPoint = new double[numDimensions];
            int index;

            double sum = 0.0;
            for (index = 0; index < numDimensions; index++)
            {
                sum += searchDirection[index] * searchDirection[index];
            }

            sum = Math.Sqrt(sum);
            if (sum > MaxStepLength)
            {
                for (index = 0; index < numDimensions; index++)
                {
                    searchDirection[index] *= MaxStepLength / sum;
                }
            }

            double slope = 0.0;
            for (index = 0; index < numDimensions; index++)
            {
                slope += startingDerivative[index] * searchDirection[index];
            }

            if (slope >= 0.0)
            {
                return Result.ErrorResult<VectorEvaluationPoint>("Rounding error.");
            }

            double test = 0.0;
            double temp;
            for (index = 0; index < numDimensions; index++)
            {
                temp = Math.Abs(searchDirection[index]) / Math.Max(startingPoint[index], 1.0);
                if (temp > test)
                {
                    test = temp;
                }
            }

            double minStepSize = 1e-7 / test;
            double currentStepSize = 1.0;
            double previousStepSize = currentStepSize;
            double candidateOutputFunctionValue = startingValue;
            double f2 = candidateOutputFunctionValue;
            double tempStepSize;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                for (index = 0; index < numDimensions; index++)
                {
                    candidateOutputPoint[index] = startingPoint[index] + currentStepSize * searchDirection[index];
                }

                candidateOutputFunctionValue = func(candidateOutputPoint);

                if (currentStepSize < minStepSize)
                {
                    for (index = 0; index < numDimensions; index++)
                    {
                        candidateOutputPoint[index] = startingPoint[index];
                    }

                    return new VectorEvaluationPoint(candidateOutputPoint, candidateOutputFunctionValue);
                }
                else if (candidateOutputFunctionValue <= startingValue + 1e-4 * currentStepSize * slope)
                {
                    return new VectorEvaluationPoint(candidateOutputPoint, candidateOutputFunctionValue);
                }
                else
                {
                    if (currentStepSize == 1.0)
                    {
                        tempStepSize = -slope / (2.0 * candidateOutputFunctionValue - startingValue - slope);
                    }
                    else
                    {
                        double rhs1 = candidateOutputFunctionValue - startingValue - currentStepSize * slope;
                        double rhs2 = f2 - startingValue - previousStepSize * slope;
                        double a = (rhs1 / (currentStepSize * currentStepSize) - rhs2 / (previousStepSize * previousStepSize)) / (currentStepSize - previousStepSize);
                        double b = (-previousStepSize * rhs1 / (currentStepSize * currentStepSize) + currentStepSize * rhs2 / (previousStepSize * previousStepSize)) / (currentStepSize - previousStepSize);
                        if (a == 0)
                        {
                            tempStepSize = -slope / (2.0 * b);
                        }
                        else
                        {
                            double disc = b * b - 3.0 * a * slope;
                            if (disc < 0.0)
                            {
                                tempStepSize = 0.5 * currentStepSize;
                            }
                            else if (b <= 0.0)
                            {
                                tempStepSize = (-b + Math.Sqrt(disc)) / (3.0 * a);
                            }
                            else
                            {
                                tempStepSize = -slope / (b + Math.Sqrt(disc));
                            }
                        }
                    }

                    if (tempStepSize > 0.5 * currentStepSize)
                    {
                        tempStepSize = 0.5 * currentStepSize;
                    }
                }

                previousStepSize = currentStepSize;
                f2 = candidateOutputFunctionValue;
                currentStepSize = Math.Max(tempStepSize, 0.1 * currentStepSize);
            }

            return Result.ErrorResult<VectorEvaluationPoint>("Exceeded max number of iterations.");
        }
    }
}