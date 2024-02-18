using System;

using Effanville.Common.Structure.Results;

namespace Effanville.Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        /// <summary>
        /// Routine to calculate a 1D root via Ridders method.
        /// </summary>
        public static class Ridders
        {
            /// <summary>
            /// Find a root using a <see cref="Type.Ridders"/> method.
            /// </summary>
            public static Result<double> FindRoot(
               Func<double, double> func,
               double lowerBound,
               double upperBound,
               int maxIterations = 30,
               double tolerance = 1e-8)
            {
                double fLower = func(lowerBound);
                double fUpper = func(upperBound);
                if ((fLower > 0.0 && fUpper < 0.0)
                    || (fLower < 0.0 && fUpper > 0.0))
                {
                    double xLower = lowerBound;
                    double xUpper = upperBound;
                    double ans = double.MinValue;
                    double xm, fm, s, xNew, fNew;
                    for (int j = 0; j < maxIterations; j++)
                    {
                        xm = 0.5 * (xLower + xUpper);
                        fm = func(xm);
                        s = Math.Sqrt(fm * fm - fLower * fUpper);
                        if (s == 0)
                        {
                            return new SuccessResult<double>(ans);
                        }

                        xNew = xm + (xm - xLower) * (fLower > fUpper ? 1.0 : -1.0) * fm / s;
                        if (Math.Abs(xNew - ans) < tolerance)
                        {
                            return new SuccessResult<double>(ans);
                        }

                        ans = xNew;
                        fNew = func(ans);
                        if (fNew == 0.0)
                        {
                            return new SuccessResult<double>(ans);
                        }

                        if (Helpers.Sign(fm, fNew) != fm)
                        {
                            xLower = xm;
                            fLower = fm;
                            xUpper = ans;
                            fUpper = fNew;
                        }
                        else if (Helpers.Sign(fLower, fNew) != fLower)
                        {
                            xUpper = ans;
                            fUpper = fNew;
                        }
                        else if (Helpers.Sign(fUpper, fNew) != fUpper)
                        {
                            xLower = ans;
                            fLower = fNew;
                        }
                        else
                        {
                            return new ErrorResult<double>("Shouldnt be here.");
                        }

                        if (Math.Abs(xUpper - xLower) < tolerance)
                        {
                            return new SuccessResult<double>(ans);
                        }
                    }

                    return new ErrorResult<double>("Exceeded max number of iterations.");
                }
                else if (fLower == 0.0)
                {
                    return new SuccessResult<double>(lowerBound);
                }
                else if (fUpper == 0.0)
                {
                    return new SuccessResult<double>(upperBound);
                }
                else
                {
                    return new ErrorResult<double>("Root must be bracketed for Ridders method to work.");
                }
            }
        }
    }
}
