using System;

using Common.Structure.MathLibrary.Matrices;
using Common.Structure.MathLibrary.Vectors;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    /// <summary>
    /// Contains methods for solving non linear equations with a BFGS solver
    /// </summary>
    public static partial class BFGS
    {
        /// <summary>
        /// Minimise the function.
        /// </summary>
        public static VectorMinResult Minimise(
            double[] startingPoint,
            double gradientTolerance,
            Func<double[], double> func,
            Func<double[], double[]> gFunc,
            double tolerance = MathConstants.TolX,
            int maxIterations = 200)
        {
            int numDimensions = startingPoint.Length;
            int dimensionIndex;
            double den, temp, test;
            double[] pnew;

            double[] candidateMinimum = startingPoint;
            double functionValue = func(candidateMinimum);
            double[] gradientValue = gFunc(candidateMinimum);

            double[,] hessian = CalculateHessian(numDimensions, null, gradientValue, null, null);

            double[] lineSearchDirection = CalculateSearchDirection(numDimensions, gradientValue, hessian);

            var lineSearcher = CreateLineSearcher(numDimensions, candidateMinimum);

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                var point = lineSearcher.FindConformingStep(candidateMinimum, functionValue, gradientValue, lineSearchDirection, func);
                pnew = point.Value.Point;
                functionValue = point.Value.Value;
                lineSearchDirection = pnew.Subtract(candidateMinimum);
                candidateMinimum = pnew;

                test = 0.0;
                for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    temp = Math.Abs(lineSearchDirection[dimensionIndex]) / Math.Max(Math.Abs(candidateMinimum[dimensionIndex]), 1.0);
                    if (temp > test)
                    {
                        test = temp;
                    }
                }

                if (test < tolerance)
                {
                    return new VectorMinResult(candidateMinimum, functionValue, ExitCondition.BoundTolerance, iteration);
                }

                double[] previousGradientValue = gradientValue;

                gradientValue = gFunc(candidateMinimum);
                den = Math.Max(functionValue, 1.0);
                for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    temp = Math.Abs(gradientValue[dimensionIndex]) * Math.Max(Math.Abs(candidateMinimum[dimensionIndex]), 1.0) / den;
                    if (temp > test)
                    {
                        test = temp;
                    }
                }

                if (test < gradientTolerance)
                {
                    return new VectorMinResult(candidateMinimum, functionValue, ExitCondition.BoundTolerance, iteration);
                }

                hessian = CalculateHessian(numDimensions, previousGradientValue, gradientValue, hessian, lineSearchDirection);
                lineSearchDirection = CalculateSearchDirection(numDimensions, gradientValue, hessian);
            }

            return new VectorMinResult(ExitCondition.ExceedIterations);
        }

        private static double[] CalculateSearchDirection(int numDimensions, double[] gradientValue, double[,] hessian)
        {
            double[] lineSearchDirection = new double[numDimensions];

            for (int dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
            {
                lineSearchDirection[dimensionIndex] = 0.0;
                for (int j = 0; j < numDimensions; j++)
                {
                    lineSearchDirection[dimensionIndex] -= hessian[dimensionIndex, j] * gradientValue[j];
                }
            }

            if (VectorFunctions.DotProduct(lineSearchDirection, gradientValue) >= 0)
            {
                lineSearchDirection = gradientValue.Negative();
            }

            return lineSearchDirection;
        }

        private static double[,] CalculateHessian(int numDimensions, double[] previousGradientValue, double[] gradientValue, double[,] previousHessian, double[] lineSearchDirection)
        {
            if (previousHessian == null)
            {
                return MatrixFunctions.Identity(numDimensions);
            }

            double[,] hessian = previousHessian;
            double[] gradientDifference = gradientValue.Subtract(previousGradientValue);
            double[] hdg = MatrixFunctions.PostMultiplyVector(previousHessian, gradientDifference);

            double fac = VectorFunctions.DotProduct(gradientDifference, lineSearchDirection);
            double fae = VectorFunctions.DotProduct(gradientDifference, hdg);
            double sumdg = VectorFunctions.DotProduct(gradientDifference, gradientDifference);
            double sumxi = VectorFunctions.DotProduct(lineSearchDirection, lineSearchDirection);

            if (fac > Math.Sqrt(MathConstants.Eps * sumdg * sumxi))
            {
                fac = 1.0 / fac;
                double fad = 1.0 / fae;
                for (int dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    gradientDifference[dimensionIndex] = fac * lineSearchDirection[dimensionIndex] - fad * hdg[dimensionIndex];
                }

                for (int dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    for (int j = dimensionIndex; j < numDimensions; j++)
                    {
                        hessian[dimensionIndex, j] += fac * lineSearchDirection[dimensionIndex] * lineSearchDirection[j]
                        - fad * hdg[dimensionIndex] * hdg[j] + fae * gradientDifference[dimensionIndex] * gradientDifference[j];
                        hessian[j, dimensionIndex] = hessian[dimensionIndex, j];
                    }
                }
            }

            return hessian;
        }
        private static ILineSearcher CreateLineSearcher(int numDimensions, double[] startingPoint)
        {
            double startingDotProduct = VectorFunctions.DotProduct(startingPoint, startingPoint);
            return new DefaultLineSearcher(MathConstants.STPMX * Math.Max(Math.Sqrt(startingDotProduct), (double)numDimensions));
        }
    }
}
