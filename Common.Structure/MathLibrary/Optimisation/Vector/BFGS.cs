using System;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    public static class BFGS
    {
        public static VectorMinResult Minimise(double[] p,
            double gTol,
            Func<double[], double> func,
            Func<double[], double[]> gFunc,
            double tolerance = MathConstants.TolX,
            int maxIterations = 200)
        {
            int n = p.Length;
            int check, i, its, j;
            double den, fac, fad, fae, fp, stpmax, sum = 0.0, sumdg, sumxi, temp, test;
            double[] dg, g, hdg;
            double[,] hessin;
            double[] pnew, xi;

            dg = new double[n];
            g = new double[n];
            hessin = new double[n, n];
            pnew = new double[n];
            xi = new double[n];
            fp = func(p);
            g = gFunc(p);
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    hessin[i, j] = 0.0;
                }
                hessin[i, i] = 1.0;
                xi[i] = -g[i];
                sum += p[i] * p[i];
            }
            stpmax = MathConstants.STPMX * FMAX(Math.Sqrt(sum), (double)n);

            for (its = 0; its < maxIterations; its++)
            {
                LineSearch();
                fp = fret;
                for (i = 0; i < n; i++)
                {
                    xi[i] = pnew[i] - p[i];
                    p[i] = pnew[i];
                }

                test = 0.0;
                for (i = 0; i < n; i++)
                {
                    temp = Math.Abs(xi[i]) / FMAX(Math.Abs(p[i]), 1.0);
                    if (temp > test)
                    {
                        test = temp;
                    }
                }

                if (test < tolerance)
                {
                    return new VectorMinResult(p,);
                }
            }

            return new VectorMinResult(ExitCondition.ExceedIterations);
        }

        private static void LineSearch()
        {
            return;
        }
    }
}
