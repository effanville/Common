
using System;
using System.Collections.Generic;

using Common.Structure.Results;

namespace Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        /// <summary>
        /// A bracket for a root.
        /// </summary>
        public sealed class RootBracket
        {
            /// <summary>
            /// The lower bound of the root.
            /// </summary>
            public double LowerBound
            {
                get;
            }

            /// <summary>
            /// The upper bound of the root.
            /// </summary>
            public double UpperBound
            {
                get;
            }

            /// <summary>
            /// Construct an instance.
            /// </summary>/
            public RootBracket(double lowerBound, double upperBound)
            {
                LowerBound = lowerBound;
                UpperBound = upperBound;
            }

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                return obj is RootBracket result &&
                       LowerBound == result.LowerBound &&
                       UpperBound == result.UpperBound;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                int hashCode = 17;
                hashCode = 23 * hashCode + LowerBound.GetHashCode();
                hashCode = 23 * hashCode + UpperBound.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Finds a pair of lower bound and upper bound for a root of a function
        /// within the given number of iterations.<br/>
        /// This expands geometrically outwards until a bracket is found.
        /// </summary>
        public static Result<RootBracket> FindBracketForRoot(
            double lowerGuess,
            double upperGuess,
            Func<double, double> func,
            int maxIterations = 50)
        {
            if (lowerGuess == upperGuess)
            {
                // bad initial range guess
                return new ErrorResult<RootBracket>($"Bad Initial guess: {nameof(lowerGuess)} = {nameof(upperGuess)}.");
            }

            double f1 = func(lowerGuess);
            double f2 = func(upperGuess);

            for (int j = 0; j < maxIterations; j++)
            {
                if (f1 * f2 < 0)
                {
                    return new SuccessResult<RootBracket>(new RootBracket(lowerGuess, upperGuess));
                }

                if (Math.Abs(f1) < Math.Abs(f2))
                {
                    f1 = func(lowerGuess += MathConstants.GoldenRatio * (lowerGuess - upperGuess));
                }
                else
                {
                    f2 = func(upperGuess += MathConstants.GoldenRatio * (upperGuess - lowerGuess));
                }
            }

            return new ErrorResult<RootBracket>("Exceeded max number of iterations.");
        }

        /// <summary>
        /// Given a function <paramref name="func"/> and bounds <paramref name="lowerBound"/> and <paramref name="upperBound"/> for a root, subdivide this region into <paramref name="numberSubDivisions"/>
        /// and return all roots found in these subdivisions. Stop once reached <paramref name="maxDesiredNumberRoots"/>.
        /// </summary>
        public static List<RootBracket> NarrowBracketForRoot(
            double lowerBound,
            double upperBound,
            Func<double, double> func,
            int numberSubDivisions,
            int maxDesiredNumberRoots)
        {
            double dx = (upperBound - lowerBound) / numberSubDivisions;
            double x = lowerBound;
            double fp = func(x);
            var brackets = new List<RootBracket>();
            for (int i = 0; i < numberSubDivisions; i++)
            {
                double fc = func(x += dx);
                if (fc * fp <= 0)
                {
                    brackets.Add(new RootBracket(x - dx, x));
                    if (brackets.Count > maxDesiredNumberRoots)
                    {
                        return brackets;
                    }
                }

                fp = fc;
            }

            return brackets;
        }
    }
}
