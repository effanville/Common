using System;

using Common.Structure.MathLibrary.Matrices;
using Common.Structure.MathLibrary.Vectors;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    /// <summary>
    /// Contains the BFGS minimisation.
    /// </summary>
    public static partial class BFGS
    {
        /// <summary>
        /// Minimise a function and its derivative.
        /// </summary>
        public static OptimisationResult<VectorFuncEvaluation> Minimise(
            double[] startingPoint,
            double gradientTolerance,
            Func<double[], double> func,
            Func<double[], double[]> gFunc,
            double tolerance = MathConstants.TolX,
            int maxIterations = 200)
        {
            int numDimensions = startingPoint.Length;

            // instantiate indexes
            int dimensionIndex, j;

            // instantiate various temp variables 
            double den, fac, fad, fae, sumdg, sumxi, temp, test;
            double[] hdg = new double[numDimensions];
            double[] pnew;
            double[] dg = new double[numDimensions];

            // The current candidate minimum.
            double[] candidateMinimum = startingPoint;

            // various function values and derivatives.
            double[,] hessian = MatrixFunctions.Identity(numDimensions);
            double[] lineSearchDirection = new double[numDimensions];
            double functionValue = func(candidateMinimum);
            double[] gradientValue = gFunc(candidateMinimum);

            // setup hessian and initial line search direction.
            for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
            {
                lineSearchDirection[dimensionIndex] = -gradientValue[dimensionIndex];
            }

            var lineSearcher = CreateLineSearcher(numDimensions, candidateMinimum);

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                // First update the search point.
                var point = lineSearcher.FindConformingStep(candidateMinimum, functionValue, gradientValue, lineSearchDirection, func);
                if (point.IsError())
                {
                    return OptimisationResult<VectorFuncEvaluation>.ErrorResult();
                }

                pnew = point.Value.Point;
                functionValue = point.Value.Value;
                for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    lineSearchDirection[dimensionIndex] = pnew[dimensionIndex] - candidateMinimum[dimensionIndex];
                    candidateMinimum[dimensionIndex] = pnew[dimensionIndex];
                }

                // test to see if convergence criteria have been met.
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
                    return new OptimisationResult<VectorFuncEvaluation>(new VectorFuncEvaluation(candidateMinimum, functionValue), ExitCondition.BoundTolerance, iteration);
                }

                for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    dg[dimensionIndex] = gradientValue[dimensionIndex];
                }

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
                    return new OptimisationResult<VectorFuncEvaluation>(new VectorFuncEvaluation(candidateMinimum, functionValue), ExitCondition.BoundTolerance, iteration);
                }

                // Now update the gradient value and hessian in preparation for the next step.
                for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    dg[dimensionIndex] = gradientValue[dimensionIndex] - dg[dimensionIndex];
                }

                for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    hdg[dimensionIndex] = 0.0;
                    for (j = 0; j < numDimensions; j++)
                    {
                        hdg[dimensionIndex] += hessian[dimensionIndex, j] * dg[j];
                    }
                }

                fac = fae = sumdg = sumxi = 0.0;
                for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    fac += dg[dimensionIndex] * lineSearchDirection[dimensionIndex];
                    fae += dg[dimensionIndex] * hdg[dimensionIndex];
                    sumdg += dg[dimensionIndex] * dg[dimensionIndex];
                    sumxi += lineSearchDirection[dimensionIndex] * lineSearchDirection[dimensionIndex];
                }

                if (fac > Math.Sqrt(MathConstants.Eps * sumdg * sumxi))
                {
                    fac = 1.0 / fac;
                    fad = 1.0 / fae;
                    for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                    {
                        dg[dimensionIndex] = fac * lineSearchDirection[dimensionIndex] - fad * hdg[dimensionIndex];
                    }

                    for (dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                    {
                        for (j = dimensionIndex; j < numDimensions; j++)
                        {
                            hessian[dimensionIndex, j] += fac * lineSearchDirection[dimensionIndex] * lineSearchDirection[j]
                            - fad * hdg[dimensionIndex] * hdg[j] + fae * dg[dimensionIndex] * dg[j];
                            hessian[j, dimensionIndex] = hessian[dimensionIndex, j];
                        }
                    }
                }

                UpdateSearchDirection(ref lineSearchDirection, numDimensions, gradientValue, hessian);
            }

            return OptimisationResult<VectorFuncEvaluation>.ExceedIterations();
        }

        private static void UpdateSearchDirection(ref double[] lineSearchDirection, int numDimensions, double[] gradientValue, double[,] hessian)
        {
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
                for (int dimensionIndex = 0; dimensionIndex < numDimensions; dimensionIndex++)
                {
                    lineSearchDirection[dimensionIndex] = -gradientValue[dimensionIndex];
                }
            }
        }

        private static ILineSearcher CreateLineSearcher(int numDimensions, double[] startingPoint)
        {
            double startingDotProduct = VectorFunctions.DotProduct(startingPoint, startingPoint);
            return new DefaultLineSearcher(MathConstants.STPMX * Math.Max(Math.Sqrt(startingDotProduct), (double)numDimensions));
        }
    }
}
