using System;

namespace Effanville.Common.Structure.MathLibrary.Optimisation.Scalar
{
    /// <summary>
    /// Contains routines for Golden section searching.
    /// </summary>
    public static class GoldenSectionSearch
    {
        /// <summary>
        /// Find a minimum of a function.
        /// </summary>
        public static Common.Structure.Results.Result<ScalarFuncEval> Minimise(
           double lowerBound,
           double upperBound,
           Func<double, double> func,
           double tolerance,
           int maxIterations)
        {
            var result = BracketMethod.BracketFromBounds(lowerBound, upperBound, func, 1000, 2, 2);
            if (result.Failure)
            {
                result = BracketMethod.Bracket(lowerBound, upperBound, func);
            }

            if (result.Failure)
            {
                return OptimisationErrorResult<ScalarFuncEval>.ErrorResult();
            }

            var value = result.Data;
            return Minimum(value.LowerPoint, value.MiddlePoint, value.UpperPoint, func, tolerance, maxIterations);
        }

        /// <summary>
        /// Find a minimum of a function with an alternative method.
        /// </summary>
        public static Common.Structure.Results.Result<ScalarFuncEval> MinimumAlternative(
           double lowerBound,
           double upperBound,
           Func<double, double> func,
           double tolerance,
           int maxIterations)
        {
            var result = BracketMethod.BracketFromBounds(lowerBound, upperBound, func, 1000, 2, 2);
            if (result.Failure)
            {
                result = BracketMethod.Bracket(lowerBound, upperBound, func);
            }

            if (result.Failure)
            {
                return OptimisationErrorResult<ScalarFuncEval>.ErrorResult();
            }

            var value = result.Data;
            return MinimumMathNet(value.LowerPoint, value.UpperPoint, func, tolerance, maxIterations);
        }

        private static Common.Structure.Results.Result<ScalarFuncEval> MinimumMathNet(
            double lowerBound,
            double upperBound,
            Func<double, double> func,
            double tolerance,
            int maxIterations)
        {
            double middlePoint = lowerBound + (upperBound - lowerBound) / (1 + MathConstants.GoldenRatio);
            int iterations = 0;
            while (Math.Abs(upperBound - lowerBound) > tolerance
                && iterations < maxIterations)
            {
                middlePoint = lowerBound + (upperBound - lowerBound) / (1 + MathConstants.GoldenRatio);
                double middleValue = func(middlePoint);

                double testPoint = lowerBound + (upperBound - middlePoint);
                double testValue = func(testPoint);
                if (testPoint < middlePoint)
                {
                    if (testValue > middleValue)
                    {
                        lowerBound = testPoint;
                    }
                    else
                    {
                        upperBound = middlePoint;
                    }
                }
                else
                {
                    if (testValue > middleValue)
                    {
                        upperBound = testPoint;
                    }
                    else
                    {
                        lowerBound = middlePoint;
                    }
                }

                iterations++;
            }

            if (iterations == maxIterations)
            {
                return OptimisationErrorResult<ScalarFuncEval>.ExceedIterations(maxIterations);
            }

            return new OptimisationSuccessResult<ScalarFuncEval>(new ScalarFuncEval(middlePoint, func(middlePoint)), ExitCondition.BoundTolerance, iterations);
        }

        private static Common.Structure.Results.Result<ScalarFuncEval> Minimum(
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
                return OptimisationErrorResult<ScalarFuncEval>.ExceedIterations(maxIterations);
            }

            if (f1 < f2)
            {
                return new OptimisationSuccessResult<ScalarFuncEval>(new ScalarFuncEval(x1, f1), ExitCondition.BoundTolerance, iterations);
            }
            else
            {
                return new OptimisationSuccessResult<ScalarFuncEval>(new ScalarFuncEval(x2, f2), ExitCondition.BoundTolerance, iterations);
            }
        }
    }
}
