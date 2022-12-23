using System;

namespace Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        /// <summary>
        /// Routine to calculate a 1D root via Van-wijngaarden Dekker Brent method.
        /// </summary>
        public static class VWDB
        {
            public static Result<double> FindRoot(
               Func<double, double> func,
               double lowerBound,
               double upperBound,
               int maxIterations = 30,
               double tolerance = 1e-8)
            {
                double a = lowerBound;
                double b = upperBound;
                double c = upperBound;
                double d = upperBound - lowerBound;
                double min1, min2;
                double e = d;

                double fa = func(a);
                double fb = func(b);
                double p, q, r, s, xm;

                if ((fa > 0.0 && fb > 0.0) ||
                    (fa < 0.0 && fb < 0.0))
                {
                    return Result.ErrorResult<double>($"Root must be bracketed for {nameof(VWDB)} to work.");
                }

                double fc = fb;

                for (int iter = 0; iter < maxIterations; iter++)
                {
                    if ((fb > 0.0 && fc > 0.0)
                        || (fb < 0.0 && fc < 0.0))
                    {
                        c = a;
                        fc = fa;
                        e = b - a;
                        d = b - a;
                    }

                    if (Math.Abs(fc) < Math.Abs(fb))
                    {
                        a = b;
                        b = c;
                        c = a;
                        fa = fb;
                        fb = fc;
                        fc = fa;
                    }

                    xm = 0.5 * (c - b);
                    if (Math.Abs(xm) <= tolerance || fb == 0.0)
                    {
                        return b;
                    }

                    if (Math.Abs(e) >= tolerance && Math.Abs(fa) > Math.Abs(fb))
                    {
                        s = fb / fa;
                        if (a == c)
                        {
                            p = 2.0 * xm * s;
                            q = 1.0 - s;
                        }
                        else
                        {
                            q = fa / fc;
                            r = fb / fc;
                            p = s * (2.0 * xm * q * (q - r) - (b - a) * (r - 1.0));
                            q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                        }

                        if (p > 0.0)
                        {
                            q = -q;
                        }

                        p = Math.Abs(p);
                        min1 = 3.0 * xm * q - Math.Abs(tolerance * q);
                        min2 = Math.Abs(e * q);
                        if (2.0 * p < (min1 < min2 ? min1 : min2))
                        {
                            e = d;
                            d = p / q;
                        }
                        else
                        {
                            d = xm;
                            e = d;
                        }
                    }
                    else
                    {
                        d = xm;
                        e = d;
                    }

                    a = b;
                    fa = fb;
                    if (Math.Abs(d) > tolerance)
                    {
                        b += d;
                    }
                    else
                    {
                        b += Helpers.Sign(tolerance, xm);
                    }

                    fb = func(b);
                }

                return Result.ErrorResult<double>("Exceeded max number of iterations.");
            }
        }
    }
}
