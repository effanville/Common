using System;

namespace Common.Structure.MathLibrary.Optimisation.Scalar
{
    public static class GoldenSectionSearch
    {
        public static ScalarMinResult Minimise(
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
                return new ScalarMinResult(double.NaN, double.NaN, ExitCondition.Error);
            }

            var value = result.Value;
            return MinimumCPPBook(value.LowerPoint, value.MiddlePoint, value.UpperPoint, func, tolerance, maxIterations);
        }

        public static ScalarMinResult MinimumFromMathNet(
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
                return new ScalarMinResult(double.NaN, double.NaN, ExitCondition.Error);
            }

            var value = result.Value;
            return MinimumMathNet(value.LowerPoint, value.UpperPoint, func, tolerance, maxIterations);
        }

        public static ScalarMinResult MinimumMathNet(
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
                return ScalarMinResult.ExceedIterations();
            }

            return new ScalarMinResult(middlePoint, func(middlePoint), ExitCondition.BoundTolerance);
        }

        private static ScalarMinResult MinimumCPPBook(
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
                return ScalarMinResult.ExceedIterations();
            }

            if (f1 < f2)
            {
                return new ScalarMinResult(x1, f1, ExitCondition.BoundTolerance);
            }
            else
            {
                return new ScalarMinResult(x2, f2, ExitCondition.BoundTolerance);
            }
        }
    }
}
