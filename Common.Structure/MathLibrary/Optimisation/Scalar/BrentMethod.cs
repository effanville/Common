using System;

namespace Common.Structure.MathLibrary.Optimisation.Scalar
{
    /// <summary>
    /// Contains methods for optimisation using the Brent Algorithm
    /// </summary>
    public static class Brent
    {
        /// <summary>
        /// Find a minimum of a function using the Brent algorithm.
        /// </summary>
        public static OptimisationResult<ScalarFuncEval> Minimise(
           double lowerBound,
           double upperBound,
           Func<double, double> func,
           double tolerance,
           int maxIterations)
        {
            var result = BracketMethod.BracketFromBounds(lowerBound, upperBound, func, maxIterations);
            if (result.IsError())
            {
                result = BracketMethod.Bracket(lowerBound, upperBound, func, maxIterations);
            }
            if (result.IsError())
            {
                return OptimisationResult<ScalarFuncEval>.ErrorResult();
            }

            var value = result.Value;
            return MinimumCPPBook(value.LowerPoint, value.MiddlePoint, value.UpperPoint, func, tolerance, maxIterations);
        }

        private static OptimisationResult<ScalarFuncEval> MinimumCPPBook(
            double lowerBound,
            double middlePoint,
            double upperBound,
            Func<double, double> func,
            double tolerance,
            int maxIterations = 100)
        {
            double a, b, etemp, fu, fv, fw, fx, p, q, r, tol1, tol2, u, v, w, x, xm;
            double d = 0.0, e = 0.0;

            a = lowerBound < upperBound ? lowerBound : upperBound;
            b = lowerBound > upperBound ? lowerBound : upperBound;
            x = w = v = middlePoint;
            fw = fv = fx = func(x);
            for (int iter = 0; iter < maxIterations; iter++)
            {
                xm = 0.5 * (a + b);
                tol2 = 2.0 * (tol1 = tolerance * Math.Abs(x) + MathConstants.ZEps);
                if (Math.Abs(x - xm) <= (tol2 - 0.5 * (b - a)))
                {
                    return new OptimisationResult<ScalarFuncEval>(new ScalarFuncEval(x, fx), ExitCondition.Converged, iter);
                }

                if (Math.Abs(e) > tol1)
                {
                    r = (x - w) * (fx - fv);
                    q = (x - v) * (fx - fw);
                    p = (x - v) * q - (x - w) * r;
                    q = 2.0 * (q - r);
                    if (q > 0.0)
                    {
                        p = -p;
                    }

                    q = Math.Abs(q);
                    etemp = e;
                    e = d;
                    if (Math.Abs(p) > Math.Abs(0.5 * q * etemp)
                        || p <= q * (a - x)
                        || p >= q * (b - x))
                    {
                        d = MathConstants.CGold * (e = (x >= xm ? a - x : b - x));
                    }
                    else
                    {
                        d = p / q;
                        u = x + d;
                        if (u - a < tol2 || b - u < tol2)
                        {
                            d = MathLibrary.Helpers.Sign(tol1, xm - x);
                        }
                    }
                }
                else
                {
                    d = MathConstants.CGold * (e = (x >= xm ? a - x : b - x));
                }

                u = Math.Abs(d) >= tol1 ? x + d : x + MathLibrary.Helpers.Sign(tol1, d);
                fu = func(u);

                if (fu <= fx)
                {
                    if (u >= x)
                    {
                        a = x;
                    }
                    else
                    {
                        b = x;
                    }

                    v = w;
                    w = x;
                    x = u;
                    fv = fw;
                    fw = fx;
                    fx = fu;
                }
                else
                {
                    if (u < x)
                    {
                        a = u;
                    }
                    else
                    {
                        b = u;
                    }

                    if (fu <= fw || w == x)
                    {
                        v = w;
                        w = u;
                        fv = fw;
                        fw = fu;
                    }
                    else if (fu <= fv || v == x || v == w)
                    {
                        v = u;
                        fv = fu;
                    }
                }
            }

            return OptimisationResult<ScalarFuncEval>.ExceedIterations(maxIterations);
        }
    }
}
