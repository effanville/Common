using System;

namespace Common.Structure.MathLibrary.Optimisation.Scalar
{
    /// <summary>
    /// Contains routines for Golden section searching.
    /// </summary>
    public static class GoldenSectionSearch
    {
        /// <summary>
        /// Find a minimum of a function.
        /// </summary>
        public static OptimisationResult<ScalarFuncEvaluation> Minimise(
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
                return OptimisationResult<ScalarFuncEvaluation>.Error();
            }

            var value = result.Value;
            return Minimum(value.LowerPoint, value.MiddlePoint, value.UpperPoint, func, tolerance, maxIterations);
        }

        private static OptimisationResult<ScalarFuncEvaluation> Minimum(
            double lowerBound,
            double middlePoint,
            double upperBound,
            Func<double, double> func,
            double tolerance,
            int maxIterations)
        {
            double lowerBoundGuess = lowerBound;
            double x1;
            double x2;
            double upperBoundGuess = upperBound;

            if (Math.Abs(upperBoundGuess - middlePoint) > Math.Abs(middlePoint - lowerBound))
            {
                x1 = middlePoint;
                x2 = middlePoint + MathConstants.C * (upperBound - middlePoint);
            }
            else
            {
                x2 = middlePoint;
                x1 = middlePoint - MathConstants.C * (middlePoint - lowerBound);
            }

            double f1 = func(x1);
            double f2 = func(x2);
            int iterations = 0;
            while (Math.Abs(upperBoundGuess - lowerBoundGuess) > tolerance
                && iterations < maxIterations)
            {
                if (f2 < f1)
                {
                    lowerBoundGuess = x1;
                    x1 = x2;
                    x2 = MathConstants.InverseGoldenRatio * x1 + MathConstants.C * upperBoundGuess;
                    f1 = f2;
                    f2 = func(x2);
                }
                else
                {
                    upperBoundGuess = x2;
                    x2 = x1;
                    x1 = MathConstants.InverseGoldenRatio * x2 + MathConstants.C * lowerBoundGuess;
                    f2 = f1;
                    f1 = func(x1);
                }

                iterations++;
            }

            if (iterations == maxIterations)
            {
                return OptimisationResult<ScalarFuncEvaluation>.ExceedIterations();
            }

            if (f1 < f2)
            {
                return new OptimisationResult<ScalarFuncEvaluation>(new ScalarFuncEvaluation(x1, f1), ExitCondition.BoundTolerance, iterations);
            }
            else
            {
                return new OptimisationResult<ScalarFuncEvaluation>(new ScalarFuncEvaluation(x2, f2), ExitCondition.BoundTolerance, iterations);
            }
        }
    }
}
