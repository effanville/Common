using System;

using Effanville.Common.Structure.Results;

namespace Effanville.Common.Structure.MathLibrary.RootFinding
{
    /// <summary>
    /// Class containing root finding algorithms.
    /// </summary>
    public static partial class RootFinder
    {
        /// <summary>
        /// Find the root of the function in the bounds specified.
        /// </summary>
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
                    return new ErrorResult<double>($"{Type.NewtonRaphson} requires a derivative function to work. Use method {nameof(NewtonRaphson)} directly.");
                }
                case Type.FalsePosition:
                {
                    return FalsePosition.FindRoot(func, lowerBound, upperBound, maxIterations, tolerance);
                }
                case Type.Ridders:
                {
                    return Ridders.FindRoot(func, lowerBound, upperBound, maxIterations, tolerance);
                }
                case Type.Secant:
                {
                    return Secant.FindRoot(func, lowerBound, upperBound, maxIterations, tolerance);
                }
                case Type.VWDB:
                {
                    return VWDB.FindRoot(func, lowerBound, upperBound, maxIterations, tolerance);
                }
                default:
                    return null;
            }
        }
    }
}
