using System;

using Effanville.Common.Structure.Results;

namespace Effanville.Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        /// <summary>
        /// Routine to calculate a 1D root via the Bisection method.
        /// </summary>
        public static class Bisection
        {
            /// <summary>
            /// Find a root with the <see cref="Type.Bisection"/> method.
            /// </summary>
            public static Result<double> FindRoot(
                Func<double, double> func,
                double lowerBound,
                double upperBound,
                int maxIterations = 100,
                double tolerance = 1e-8)
            {
                double f = func(lowerBound);
                double fMid = func(upperBound);
                double dx;
                double xMid;
                if (f * fMid >= 0)
                {
                    return new ErrorResult<double>("Root must be bracketed for bisection to work.");
                }
                double rtb;
                if (f < 0)
                {
                    dx = upperBound - lowerBound;
                    rtb = lowerBound;
                }
                else
                {
                    dx = lowerBound - upperBound;
                    rtb = upperBound;
                }
                for (int j = 0; j < maxIterations; j++)
                {
                    dx *= 0.5;
                    xMid = rtb + dx;
                    fMid = func(xMid);
                    if (fMid <= 0)
                    {
                        rtb = xMid;
                    }
                    if (Math.Abs(dx) < tolerance || fMid == 0)
                    {
                        return new SuccessResult<double>(rtb);
                    }
                }

                return new ErrorResult<double>("Exceeded max number of iterations.");
            }
        }
    }
}
