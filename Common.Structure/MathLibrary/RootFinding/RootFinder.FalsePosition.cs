using System;

using Common.Structure.Results;

namespace Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        /// <summary>
        /// Routine to calculate a 1D root via the FalsePosition method.
        /// </summary>
        public static class FalsePosition
        {
            /// <summary>
            /// Find the root using a <see cref="Type.FalsePosition"/> method.
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
                double xLower;
                double xUpper;
                double swap;
                if (fLower * fUpper > 0.0)
                {
                    return new ErrorResult<double>("Root must be bracketed for False Position to work.");
                }

                if (fLower < 0.0)
                {
                    xLower = lowerBound;
                    xUpper = upperBound;
                }
                else
                {
                    xLower = upperBound;
                    xUpper = lowerBound;
                    swap = fLower;
                    fLower = fUpper;
                    fUpper = swap;
                }

                double dx = xUpper - xLower;
                double rootCandidate;
                double fRootCandidate;
                double del;
                for (int j = 0; j < maxIterations; j++)
                {
                    rootCandidate = xLower + dx * fLower / (fLower - fUpper);
                    fRootCandidate = func(rootCandidate);
                    if (fRootCandidate < 0.0)
                    {
                        del = xLower - rootCandidate;
                        xLower = rootCandidate;
                        fLower = fRootCandidate;
                    }
                    else
                    {
                        del = xUpper - rootCandidate;
                        xUpper = rootCandidate;
                        fUpper = fRootCandidate;
                    }

                    dx = xUpper - xLower;
                    if (Math.Abs(del) < tolerance || fRootCandidate == 0.0)
                    {
                        return new SuccessResult<double>(rootCandidate);
                    }
                }

                return new ErrorResult<double>("Exceeded max number of iterations.");
            }
        }
    }
}
