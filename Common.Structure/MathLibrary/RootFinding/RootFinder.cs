using System;

namespace Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        public enum Type
        {
            Bisection,
            NewtonRaphson,
        }

        public static Result<double> FindRoot(
            Type rootFinderType,
            Func<double, double> func,
            double lowerBound,
            double upperBound,
            int maxIterations = 100,
            double tolerance = 1e-8)
        {
            switch (rootFinderType)
            {
                case Type.Bisection:
                {
                    return Bisection.FindRoot(func, lowerBound, upperBound, maxIterations, tolerance);
                }
                case Type.NewtonRaphson:
                {
                    return Result.ErrorResult<double>($"{Type.NewtonRaphson} requires a derivative function to work. Use method {nameof(NewtonRaphson)} directly.");
                }
                default:
                    return null;
            }
        }
    }
}
