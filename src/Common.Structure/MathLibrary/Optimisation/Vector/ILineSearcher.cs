﻿using System;

namespace Effanville.Common.Structure.MathLibrary.Optimisation.Vector
{
    /// <summary>
    /// Routine for searching along a line.
    /// </summary>
    public interface ILineSearcher
    {
        /// <summary>
        /// Finds a point on a line where the function value has decreased
        /// sufficiently from the starting point.
        /// </summary>
        /// <param name="startingPoint">The point to start the move from.</param>
        /// <param name="startingValue">The value at the starting point.</param>
        /// <param name="startingDerivative">The derivative at the starting point.</param>
        /// <param name="searchDirection">The direction to move in.</param>
        /// <param name="func">The function.</param>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <returns>The point at value </returns>
        Common.Structure.Results.Result<VectorFuncEval> FindConformingStep(
            double[] startingPoint,
            double startingValue,
            double[] startingDerivative,
            double[] searchDirection,
            Func<double[], double> func,
            int maxIterations = 100);

        /// <summary>
        /// Finds a point on a line where the function value has decreased
        /// sufficiently from the starting point.
        /// </summary>
        /// <param name="startingPoint">The point to start the move from.</param>
        /// <param name="startingValue">The value at the starting point.</param>
        /// <param name="startingDerivative">The derivative at the starting point.</param>
        /// <param name="searchDirection">The direction to move in.</param>
        /// <param name="upperBound">The largest value to move in the direction.</param>
        /// <param name="func">The function.</param>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <returns>The point at value </returns>
        Common.Structure.Results.Result<VectorFuncEval> FindConformingStep(
            double[] startingPoint,
            double startingValue,
            double[] startingDerivative,
            double[] searchDirection,
            double upperBound,
            Func<double[], double> func,
            int maxIterations = 100);
    }
}
