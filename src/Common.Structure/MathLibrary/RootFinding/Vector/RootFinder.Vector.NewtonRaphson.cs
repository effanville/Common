﻿using System;

using Effanville.Common.Structure.MathLibrary.Matrices;
using Effanville.Common.Structure.Results;

namespace Effanville.Common.Structure.MathLibrary.RootFinding
{
    public static partial class RootFinder
    {
        /// <summary>
        /// Contains vector root finder methods.
        /// </summary>
        public static partial class Vector
        {
            /// <summary>
            /// Calculates a root via the Newton Raphson method.
            /// </summary>
            public static class NewtonRaphson
            {
                /// <summary>
                /// Find a vector root using the Newton Raphson method.
                /// </summary>
                public static Result<double[]> FindRoot(
                    Func<double[], double[]> func,
                    Func<double[], double[,]> derivativeFunc,
                    double[] initialGuess,
                    int maxIterations = 100,
                    double tolerance = 1e-8,
                    double toleranceFunction = 1e-8
                    )
                {
                    int n = initialGuess.Length;
                    double[] x = initialGuess;
                    for (int k = 0; k < maxIterations; k++)
                    {
                        double[] f = func(x);
                        double[,] df = derivativeFunc(x);
                        double errf = 0.0;
                        for (int i = 0; i < n; i++)
                        {
                            errf += Math.Abs(f[i]);
                        }

                        if (errf <= toleranceFunction)
                        {
                            return new SuccessResult<double[]>(x);
                        }

                        double[] p = new double[n];
                        for (int i = 0; i < n; i++)
                        {
                            p[i] = -f[i];
                        }

                        Result<LUDecomposition> luDecomp = LUDecomposition.Generate(df);
                        if (luDecomp.Failure)
                        {
                            return new ErrorResult<double[]>("Could not decompose");
                        }

                        double[] output = luDecomp.Data.LinearSolve(p);
                        double errx = 0.0;
                        for (int i = 0; i < n; i++)
                        {
                            errx += Math.Abs(output[i]);
                            x[i] += output[i];
                        }

                        if (errx < tolerance)
                        {
                            return new SuccessResult<double[]>(x);
                        }
                    }

                    return new ErrorResult<double[]>("Exceeded max number of iterations.");
                }
            }
        }
    }
}
