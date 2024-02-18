using System;

namespace Effanville.Common.Structure.MathLibrary.Functions
{
    /// <summary>
    /// The incomplete Gamma function written as P(a,x).
    /// See Numerical Recipes in C third edition p261
    /// </summary>
    public static class Gamma
    {
        private const int ASwitch = 100;
        private const double eps = double.Epsilon;
        private const double fpMin = eps;

        /// <summary>
        /// Evaluates the incomplete gamma function  with upper limit on integral of x
        /// </summary>
        public static double GammaP(double a, double x)
        {
            if (x < 0.0 || a <= 0.0)
            {
                return double.NaN;
            }

            if (x.Equals(0))
            {
                return 0.0;
            }
            else if (a >= ASwitch)
            {
                return GammaPApprox(a, x, 1);
            }
            else if (x < a + 1.0)
            {
                return GammaFromSeries(a, x);
            }
            else
            {
                return GammaFromContinuedFraction(a, x);
            }
        }

        /// <summary>
        /// Returns 1-GammaP (a,x)
        /// </summary>
        public static double GammaQ(double a, double x)
        {
            if (x < 0.0 || a <= 0.0)
            {
                return double.NaN;
            }

            if (x.Equals(0))
            {
                return 1.0;
            }
            else if (a >= ASwitch)
            {
                return GammaPApprox(a, x, 0);
            }
            else if (x < a + 1.0)
            {
                return 1.0 - GammaFromSeries(a, x);
            }
            else
            {
                return 1.0 - GammaFromContinuedFraction(a, x);
            }
        }

        /// <summary>
        /// The inverse function of GammaP, given fixed a.
        /// </summary>
        public static double InverseGammaP(double p, double a)
        {
            double x, err;
            double eps = 1.0e-8;
            double lna1 = 0;
            double a1 = a - 1;
            double afac = 0.0;
            double pp;
            double t;
            double u;
            double logGamma = BasicFunctions.LogGamma(a);

            if (a <= 0.0)
            {
                return double.NaN;
            }

            if (p >= 1.0)
            {
                return Math.Max(100.0, a + 100 * Math.Sqrt(a));
            }

            if (p <= 0.0)
            {
                return 0.0;
            }

            if (a > 1.0)
            {
                lna1 = Math.Log(a1);
                afac = Math.Exp(a1 * (lna1 - 1.0) - logGamma);
                pp = (p < 0.5) ? p : 1.0 - p;
                t = Math.Sqrt(-2 * Math.Log(pp));
                x = (2.30753 + t * 0.27061) / (1.0 + t * (0.99229 + t * 0.04481)) - t;

                if (p < 0.5)
                {
                    x = -x;
                }
                x = Math.Max(1.0e-3, a * Math.Pow(1.0 - 1.0 / (9.0 * a) - x / (3.0 * Math.Sqrt(a)), 3));
            }
            else
            {
                t = 1.0 - a * (-.253 + a * 0.12);
                if (p < t)
                {
                    x = Math.Pow(p / t, 1.0 / a);
                }
                else
                {
                    x = 1.0 - Math.Log(1.0 - (p - t) / (1.0 - t));
                }
            }

            for (int iteration = 0; iteration < 12; iteration++)
            {
                if (x <= 0.0)
                {
                    return 0.0;
                }

                err = GammaP(a, x) - p;
                if (a > 1.0)
                {
                    t = afac * Math.Exp(-(x - a1) + a1 * (Math.Log(x) - lna1));
                }
                else
                {
                    t = Math.Exp(-x + a1 * Math.Log(x) - logGamma);
                }

                u = err / t;
                x -= (t = u / (1.0 - 0.5 * Math.Min(1.0, u * ((a - 1.0) / x - 1))));
                if (x <= 0)
                {
                    x = 0.5 * (x + t);
                }

                if (Math.Abs(t) < eps * x)
                {
                    break;
                }
            }

            return x;
        }

        private static double GammaFromSeries(double a, double x)
        {
            double sum = 1.0 / a;
            double del = 1.0 / a;
            double ap = a;
            double logGamma = BasicFunctions.LogGamma(a);

            do
            {
                ++ap;
                del *= x / ap;
                sum += del;
            } while (Math.Abs(del) < Math.Abs(sum) * eps);

            return sum * Math.Exp(-x + a * Math.Log(x) - logGamma);
        }

        private static double GammaFromContinuedFraction(double a, double x)
        {
            double an, del;
            double logGamma = BasicFunctions.LogGamma(a);
            double b = x + 1.0 - a;
            double c = 1.0 / fpMin;
            double d = 1.0 / b;
            double h = d;

            for (int iteration = 0; ; iteration++)
            {
                an = -1 * (iteration - a);
                b += 2.0;
                d = an * d + b;

                if (Math.Abs(d) < fpMin)
                {
                    d = fpMin;
                }
                c = b + an / c;
                if (Math.Abs(c) < fpMin)
                {
                    c = fpMin;
                }

                d = 1.0 / d;

                del = d * c;

                h *= del;

                if (Math.Abs(del - 1.0) < eps)
                {
                    break;
                }
            }

            return Math.Exp(-x + a * Math.Log(x) - logGamma) * h;
        }

        private static double GammaPApprox(double a, double x, int n)
        {
            double xu;
            double t, sum = 0, ans;
            double a1 = a - 1.0;
            double loga1 = Math.Log(a1);
            double sqrta1 = Math.Sqrt(a1);
            double logGamma = BasicFunctions.LogGamma(a);

            if (x > a1)
            {
                xu = Math.Max(a1 + 11.5 * sqrta1, x + 6.0 * sqrta1);
            }
            else
            {
                xu = Math.Max(0.0, Math.Min(a1 - 7.5 * sqrta1, x - 5.0 * sqrta1));
            }

            for (int gaussIndex = 0; gaussIndex < GaussLegendreQuadrature.NumberValues; gaussIndex++)
            {
                t = x + (xu - x) * GaussLegendreQuadrature.Abscissa[gaussIndex];
                sum += GaussLegendreQuadrature.Weights[gaussIndex] * Math.Exp(-(t - a1) + a1 * (Math.Log(t) - loga1));
            }

            ans = sum * (xu - x) * Math.Exp(a1 * (loga1 - 1.0) - logGamma);
            return (n == 0) ? (ans > 0 ? 1.0 - ans : -ans) : (ans >= 0.0 ? ans : 1.0 + ans);
        }
    }
}
