using System;

using Common.Structure.Results;

namespace Common.Structure.MathLibrary.Optimisation.Scalar
{
    /// <summary>
    /// Contains routines for calculating a bracket of an extremum.
    /// </summary>
    public static class BracketMethod
    {
        /// <summary>
        /// A bracket of an extremum.
        /// </summary>
        public sealed class ExtremumBracket
        {
            /// <summary>
            /// The lower point of the bracket.
            /// </summary>
            public double LowerPoint
            {
                get;
            }

            /// <summary>
            /// The middle point of the bracket.
            /// </summary>
            public double MiddlePoint
            {
                get;
            }

            /// <summary>
            /// The upper point of the bracket.
            /// </summary>
            public double UpperPoint
            {
                get;
            }

            /// <summary>
            /// The lower function output value of the bracket.
            /// </summary>
            public double LowerValue
            {
                get;
            }

            /// <summary>
            /// The middle point function value of the bracket.
            /// </summary>
            public double MiddleValue
            {
                get;
            }

            /// <summary>
            /// The upper point function value of the bracket.
            /// </summary>
            public double UpperValue
            {
                get;
            }

            /// <summary>
            /// Construct an instance of a <see cref="ExtremumBracket"/>
            /// </summary>
            public ExtremumBracket(
                double lowerPoint,
                double middlePoint,
                double upperPoint,
                double lowerValue,
                double middleValue,
                double upperValue)
            {
                LowerPoint = lowerPoint;
                MiddlePoint = middlePoint;
                UpperPoint = upperPoint;
                LowerValue = lowerValue;
                MiddleValue = middleValue;
                UpperValue = upperValue;
            }

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                return obj is ExtremumBracket result
                    && LowerPoint == result.LowerPoint
                    && MiddlePoint == result.MiddlePoint
                    && UpperPoint == result.UpperPoint
                    && LowerValue == result.LowerValue
                    && MiddleValue == result.MiddleValue
                    && UpperValue == result.UpperValue;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                int hashCode = 17;
                hashCode = 23 * hashCode + LowerPoint.GetHashCode();
                hashCode = 23 * hashCode + MiddlePoint.GetHashCode();
                hashCode = 23 * hashCode + UpperPoint.GetHashCode();
                hashCode = 23 * hashCode + LowerValue.GetHashCode();
                hashCode = 23 * hashCode + MiddleValue.GetHashCode();
                hashCode = 23 * hashCode + UpperValue.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Calculate a bracket from an initial bound guess. Starts with the bounds, then expands outwards until 
        /// one does bracket the extremum.
        /// </summary>
        public static Result<ExtremumBracket> BracketFromBounds(
            double lowerBound,
            double upperBound,
            Func<double, double> func,
            int maxIterations,
            double lowerExpansionFactor = 2,
            double upperExpansionFactor = 2)
        {
            double middlePoint = lowerBound + (upperBound - lowerBound) / (1 + MathConstants.GoldenRatio);
            int iteration = 0;

            double lowerValue = func(lowerBound);
            double middleValue = func(middlePoint);
            double upperValue = func(upperBound);
            while (iteration < maxIterations
                && upperValue < middleValue || lowerValue < middleValue)
            {
                if (lowerValue < middleValue)
                {
                    lowerBound = 0.5 * (upperBound + lowerBound) - lowerExpansionFactor * 0.5 * (upperBound - lowerBound);
                    lowerValue = func(lowerBound);
                }

                if (upperValue < middleValue)
                {
                    upperBound = 0.5 * (upperBound + lowerBound) + upperExpansionFactor * 0.5 * (upperBound - lowerBound);
                    upperValue = func(upperBound);
                }

                middlePoint = lowerBound + (upperBound - lowerBound) / (1 + MathConstants.GoldenRatio);
                middleValue = func(middlePoint);
                iteration++;
            }

            if (upperValue < middleValue || lowerValue < middleValue)
            {
                return new ErrorResult<ExtremumBracket>("Could not bracket minimum value.");
            }

            return new SuccessResult<ExtremumBracket>(new ExtremumBracket(lowerBound, middlePoint, upperBound, lowerValue, middleValue, upperValue));
        }

        /// <summary>
        /// Calculate a bracket from an initial bound guess. Starts with the bounds, then expands outwards until 
        /// one does bracket the extremum.
        /// </summary>
        public static Result<ExtremumBracket> Bracket(
            double lowerStartingPoint,
            double upperStartingPoint,
            Func<double, double> func,
            int maxIterations = 100)
        {
            double lowerPoint, middlePoint, upperPoint, lowerValue, middleValue, upperValue;
            double GLimit = 100.0;
            lowerPoint = lowerStartingPoint;
            middlePoint = upperStartingPoint;
            double fu;
            lowerValue = func(lowerPoint);
            middleValue = func(middlePoint);
            if (middleValue > lowerValue)
            {
                (middleValue, lowerValue) = (lowerValue, middleValue);
                (middlePoint, lowerPoint) = (lowerPoint, middlePoint);
            }

            upperPoint = middlePoint + MathConstants.GoldenRatio + (middlePoint - lowerPoint);
            upperValue = func(upperPoint);
            int iterations = 0;
            while (middleValue > upperValue && iterations < maxIterations)
            {
                double r = (middlePoint - lowerPoint) * (middleValue - upperValue);
                double q = (middlePoint - upperPoint) * (middleValue - lowerValue);
                double u = middlePoint - ((middlePoint - upperPoint) * q - (middlePoint - lowerPoint) * r) / (2.0 * Math.Abs(Math.Max(Math.Abs(q - r), MathConstants.Tiny)) * Math.Sign(q - r));
                double ulim = middlePoint + GLimit * (upperPoint - middlePoint);
                if ((middlePoint - u) * (u - upperPoint) > 0.0)
                {
                    fu = func(u);
                    if (fu < upperValue)
                    {
                        lowerPoint = middlePoint;
                        middlePoint = u;
                        lowerValue = middleValue;
                        middleValue = fu;
                        return new SuccessResult<ExtremumBracket>(new ExtremumBracket(lowerPoint, middlePoint, upperPoint, lowerValue, middleValue, upperValue));
                    }
                    else if (fu > middleValue)
                    {
                        upperPoint = u;
                        upperValue = fu;
                        return new SuccessResult<ExtremumBracket>(new ExtremumBracket(lowerPoint, middlePoint, upperPoint, lowerValue, middleValue, upperValue));
                    }
                    else
                    {
                        u = upperPoint + MathConstants.GoldenRatio * (upperPoint - middlePoint);
                        fu = func(u);
                    }
                }
                else if ((upperPoint - u) * (u - ulim) > 0.0)
                {
                    fu = func(u);
                    if (fu < upperValue)
                    {
                        middlePoint = upperPoint;
                        upperPoint = u;
                        u = upperPoint + MathConstants.GoldenRatio * (upperPoint - middlePoint);
                        middleValue = upperValue;
                        upperValue = fu;
                        fu = func(u);
                    }
                }
                else if ((u - ulim) * (ulim - upperPoint) >= 0.0)
                {
                    u = ulim;
                    fu = func(u);
                }
                else
                {
                    u = upperPoint + MathConstants.GoldenRatio * (upperPoint - middlePoint);
                    fu = func(u);
                }

                Helpers.ShiftValues(ref lowerPoint, ref middlePoint, ref upperPoint, u);
                Helpers.ShiftValues(ref lowerValue, ref middleValue, ref upperValue, fu);
                iterations++;
            }

            if (iterations > maxIterations)
            {
                return new ErrorResult<ExtremumBracket>("Exceeded maximum number of iterations.");
            }

            return new SuccessResult<ExtremumBracket>(new ExtremumBracket(lowerPoint, middlePoint, upperPoint, lowerValue, middleValue, upperValue));
        }
    }
}
