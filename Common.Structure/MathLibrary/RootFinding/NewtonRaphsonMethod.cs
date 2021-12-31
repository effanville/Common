using System;

namespace Common.Structure.MathLibrary.RootFinding
{
    /// <summary>
    /// Calculates a root via the Newton Raphson method.
    /// </summary>
    public static class NewtonRaphsonMethod
    {
        /// <summary>
        /// Calculates a root via the Newton Raphson method in an unsafe manner.
        /// This may or may not converge based upon the shape of the function and 
        /// the initial bracket.
        /// </summary>
        public static Result<double> FindRootUnsafe(
            Func<double, double> func,
            Func<double, double> derivativeFunc,
            double lowerBound,
            double upperBound,
            int maxIterations = 100,
            double tolerance = 1e-8)
        {
            double rtn = 0.5 * (lowerBound + upperBound);
            double dx;
            double f;
            double Gradf;
            for (int j = 0; j < maxIterations; j++)
            {
                f = func(rtn);
                Gradf = derivativeFunc(f);
                dx = f / Gradf;
                rtn -= dx;
                if ((lowerBound - rtn) * (rtn - upperBound) < 0.0)
                {
                    return Result.ErrorResult<double>("Jumped out of bracketing interval.");
                }

                if (Math.Abs(dx) < tolerance)
                {
                    return rtn;
                }
            }

            return Result.ErrorResult<double>("Exceeded max iterations.");
        }

        /// <summary>
        /// Calculates a root via the Newton Raphson method in a safe manner.
        /// If the Newton step is out of bounds, this then performs a bisection.
        /// </summary>
        public static Result<double> FindRootSafe(
            Func<double, double> func,
            Func<double, double> derivativeFunc,
            double lowerBound,
            double upperBound,
            int maxIterations = 100,
            double tolerance = 1e-8)
        {
            double fl = func(lowerBound);
            double fh = func(upperBound);
            if ((fl > 0.0 && fh > 0.0) || (fl < 0.0 && fh < 0.0))
            {
                return Result.ErrorResult<double>("Root was not bracketed by initial guess.");
            }

            if (Math.Abs(fl) < tolerance)
            {
                return lowerBound;
            }

            if (Math.Abs(fh) < tolerance)
            {
                return upperBound;
            }

            double xl;
            double xh;
            if (fl < 0)
            {
                xl = lowerBound;
                xh = upperBound;
            }
            else
            {
                xh = lowerBound;
                xl = upperBound;
            }

            double rts = 0.5 * (xl + xh);
            double dxOld = Math.Abs(xh - xl);
            double dx = dxOld;
            double f = func(rts);
            double gradf = derivativeFunc(rts);
            double temp;
            for (int j = 0; j < maxIterations; j++)
            {
                if ((((rts - xh) * gradf - f) * ((rts - xl) * gradf - f) > 0.0)
                    || (Math.Abs(2.0 * f) > Math.Abs(dxOld * gradf)))
                {
                    dxOld = dx;
                    dx = 0.5 * (xh - xl);
                    rts = xl + dx;
                    if (xl == rts)
                    {
                        return rts;
                    }
                }
                else
                {
                    dxOld = dx;
                    dx = f / gradf;
                    temp = rts;
                    rts -= dx;
                    if (temp == rts)
                    {
                        return rts;
                    }
                }

                if (Math.Abs(dx) < tolerance)
                {
                    return rts;
                }

                f = func(rts);
                gradf = derivativeFunc(rts);
                if (f < 0.0)
                {
                    xl = rts;
                }
                else
                {
                    xh = rts;
                }
            }

            return Result.ErrorResult<double>("Exceeded max iterations.");
        }
    }
}
