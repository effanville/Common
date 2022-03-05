using System;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    public static partial class BFGS
    {
        public static VectorMinResult Minimise(
            double[] startingPoint,
            double gradientTolerance,
            Func<double[], double> func,
            Func<double[], double[]> gFunc,
            double tolerance = MathConstants.TolX,
            int maxIterations = 200)
        {
            int n = startingPoint.Length;
            int dimensionIndex, iteration, j;
            double den, fac, fad, fae, functionValue, stpmax, sum = 0.0, sumdg, sumxi, temp, test;
            double[] dg, gradientValue;
            double[] hdg = new double[n];
            double[,] hessian;
            double[] pnew, lineSearchDirection;

            dg = new double[n];
            hessian = new double[n, n];
            lineSearchDirection = new double[n];
            functionValue = func(startingPoint);
            gradientValue = gFunc(startingPoint);
            for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
            {
                for (j = 0; j < n; j++)
                {
                    hessian[dimensionIndex, j] = 0.0;
                }
                hessian[dimensionIndex, dimensionIndex] = 1.0;
                lineSearchDirection[dimensionIndex] = -gradientValue[dimensionIndex];
                sum += startingPoint[dimensionIndex] * startingPoint[dimensionIndex];
            }

            stpmax = MathConstants.STPMX * Math.Max(Math.Sqrt(sum), (double)n);
            var lineSearcher = new DefaultLineSearcher(stpmax);

            for (iteration = 0; iteration < maxIterations; iteration++)
            {
                var point = lineSearcher.FindConformingStep(startingPoint, functionValue, gradientValue, lineSearchDirection, func);
                pnew = point.Value.Point;
                functionValue = point.Value.Value;
                for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                {
                    lineSearchDirection[dimensionIndex] = pnew[dimensionIndex] - startingPoint[dimensionIndex];
                    startingPoint[dimensionIndex] = pnew[dimensionIndex];
                }

                test = 0.0;
                for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                {
                    temp = Math.Abs(lineSearchDirection[dimensionIndex]) / Math.Max(Math.Abs(startingPoint[dimensionIndex]), 1.0);
                    if (temp > test)
                    {
                        test = temp;
                    }
                }

                if (test < tolerance)
                {
                    return new VectorMinResult(startingPoint, functionValue, ExitCondition.BoundTolerance, iteration);
                }

                for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                {
                    dg[dimensionIndex] = gradientValue[dimensionIndex];
                }

                gradientValue = gFunc(startingPoint);
                den = Math.Max(functionValue, 1.0);
                for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                {
                    temp = Math.Abs(gradientValue[dimensionIndex]) * Math.Max(Math.Abs(startingPoint[dimensionIndex]), 1.0) / den;
                    if (temp > test)
                    {
                        test = temp;
                    }
                }

                if (test < gradientTolerance)
                {
                    return new VectorMinResult(startingPoint, functionValue, ExitCondition.BoundTolerance, iteration);
                }

                for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                {

                    dg[dimensionIndex] = gradientValue[dimensionIndex] - dg[dimensionIndex];
                }

                for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                {
                    hdg[dimensionIndex] = 0.0;
                    for (j = 0; j < n; j++)
                    {
                        hdg[dimensionIndex] += hessian[dimensionIndex, j] * dg[j];
                    }
                }

                fac = fae = sumdg = sumxi = 0.0;
                for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
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
                    for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                    {
                        dg[dimensionIndex] = fac * lineSearchDirection[dimensionIndex] - fad * hdg[dimensionIndex];
                    }

                    for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                    {
                        for (j = dimensionIndex; j < n; j++)
                        {
                            hessian[dimensionIndex, j] += fac * lineSearchDirection[dimensionIndex] * lineSearchDirection[j]
                            - fad * hdg[dimensionIndex] * hdg[j] + fae * dg[dimensionIndex] * dg[j];
                            hessian[j, dimensionIndex] = hessian[dimensionIndex, j];
                        }
                    }
                }

                for (dimensionIndex = 0; dimensionIndex < n; dimensionIndex++)
                {
                    lineSearchDirection[dimensionIndex] = 0.0;
                    for (j = 0; j < n; j++)
                    {
                        lineSearchDirection[dimensionIndex] -= hessian[dimensionIndex, j] * gradientValue[j];
                    }
                }
            }

            return new VectorMinResult(ExitCondition.ExceedIterations);
        }
    }
}
