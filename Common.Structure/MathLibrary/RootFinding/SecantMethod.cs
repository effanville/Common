using System;

namespace Common.Structure.MathLibrary.RootFinding
{
    /// <summary>
    /// Routine to calculate a 1D root via the Secant method.
    /// </summary>
    public static class SecantMethod
    {
        public static Result<double> Root(
           Func<double, double> func,
           double lowerBound,
           double upperBound,
           int maxIterations = 30,
           double tolerance = 1e-8)
        {
            double fLower = func(lowerBound);
            double f = func(upperBound);
            double xLower;
            double swap;
            double rootCandidate;
            if (Math.Abs(fLower) < Math.Abs(f))
            {
                rootCandidate = lowerBound;
                xLower = upperBound;
                swap = fLower;
                fLower = f;
                f = swap;
            }
            else
            {
                xLower = lowerBound;
                rootCandidate = upperBound;
            }
            double dx;
            for (int j = 0; j < maxIterations; j++)
            {
                dx = (xLower - rootCandidate) * f / (f - fLower);
                xLower = rootCandidate;
                fLower = f;
                rootCandidate += dx;
                f = func(rootCandidate);
                if (Math.Abs(dx) < tolerance || f == 0.0)
                {
                    return rootCandidate;
                }
            }

            return Result.ErrorResult<double>("Exceeded max number of iterations.");
        }
    }
}
