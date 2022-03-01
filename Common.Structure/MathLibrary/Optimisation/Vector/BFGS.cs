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
            int i, its, j;
            double den, fac, fad, fae, fp, stpmax, sum = 0.0, sumdg, sumxi, temp, test;
            double[] dg, g;
            double[] hdg = new double[n];
            double[,] hessin;
            double[] pnew, xi;

            dg = new double[n];
            hessin = new double[n, n];
            xi = new double[n];
            fp = func(startingPoint);
            g = gFunc(startingPoint);
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    hessin[i, j] = 0.0;
                }
                hessin[i, i] = 1.0;
                xi[i] = -g[i];
                sum += startingPoint[i] * startingPoint[i];
            }

            stpmax = MathConstants.STPMX * Math.Max(Math.Sqrt(sum), (double)n);
            var lineSearcher = new DefaultLineSearcher(stpmax);

            for (its = 0; its < maxIterations; its++)
            {
                var point = lineSearcher.FindConformingStep(startingPoint, fp, g, xi, func);
                pnew = point.Value.Point;
                fp = point.Value.Value;
                for (i = 0; i < n; i++)
                {
                    xi[i] = pnew[i] - startingPoint[i];
                    startingPoint[i] = pnew[i];
                }

                test = 0.0;
                for (i = 0; i < n; i++)
                {
                    temp = Math.Abs(xi[i]) / Math.Max(Math.Abs(startingPoint[i]), 1.0);
                    if (temp > test)
                    {
                        test = temp;
                    }
                }

                if (test < tolerance)
                {
                    return new VectorMinResult(startingPoint, fp, ExitCondition.BoundTolerance, its);
                }

                for (i = 0; i < n; i++)
                {
                    dg[i] = g[i];
                }

                g = gFunc(startingPoint);
                den = Math.Max(fp, 1.0);
                for (i = 0; i < n; i++)
                {
                    temp = Math.Abs(g[i]) * Math.Max(Math.Abs(startingPoint[i]), 1.0) / den;
                    if (temp > test)
                    {
                        test = temp;
                    }
                }

                if (test < gradientTolerance)
                {
                    return new VectorMinResult(startingPoint, fp, ExitCondition.BoundTolerance, its);
                }

                for (i = 0; i < n; i++)
                {

                    dg[i] = g[i] - dg[i];
                }

                for (i = 0; i < n; i++)
                {
                    hdg[i] = 0.0;
                    for (j = 0; j < n; j++)
                    {
                        hdg[i] += hessin[i, j] * dg[j];
                    }
                }

                fac = fae = sumdg = sumxi = 0.0;
                for (i = 0; i < n; i++)
                {
                    fac += dg[i] * xi[i];
                    fae += dg[i] * hdg[i];
                    sumdg += dg[i] * dg[i];
                    sumxi += xi[i] * xi[i];
                }

                if (fac > Math.Sqrt(MathConstants.Eps * sumdg * sumxi))
                {
                    fac = 1.0 / fac;
                    fad = 1.0 / fae;
                    for (i = 0; i < n; i++)
                    {
                        dg[i] = fac * xi[i] - fad * hdg[i];
                    }

                    for (i = 0; i < n; i++)
                    {
                        for (j = i; j < n; j++)
                        {
                            hessin[i, j] += fac * xi[i] * xi[j]
                            - fad * hdg[i] * hdg[j] + fae * dg[i] * dg[j];
                            hessin[j, i] = hessin[i, j];
                        }
                    }
                }

                for (i = 0; i < n; i++)
                {
                    xi[i] = 0.0;
                    for (j = 0; j < n; j++)
                    {
                        xi[i] -= hessin[i, j] * g[j];
                    }
                }
            }

            return new VectorMinResult(ExitCondition.ExceedIterations);
        }
    }
}
