using System;

namespace Common.Structure.MathLibrary.RootFinding
{
    public static class BisectionMethod
    {
        public static Result<double> Root(
            Func<double, double> func,
            double lowerBound,
            double upperBound,
            int maxIterations,
            double tolerance)
        {
            double f = func(lowerBound);
            double fMid = func(upperBound);
            double dx;
            double xMid;
            if (f * fMid > 0)
            {
                return Result.ErrorResult<double>("Root must be bracketed for bisection to work.");
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
                fMid = func(xMid = rtb + (dx *= 0.5));
                if (fMid < 0)
                {
                    rtb = xMid;
                }
                if (Math.Abs(dx) < tolerance || fMid == 0)
                {
                    return rtb;
                }
            }

            return Result.ErrorResult<double>("Exceeded max number of iterations.");
        }
    }
}
