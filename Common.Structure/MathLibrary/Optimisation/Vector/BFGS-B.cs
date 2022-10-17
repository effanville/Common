using System;

using Common.Structure.MathLibrary.Matrices;
using Common.Structure.MathLibrary.Vectors;

namespace Common.Structure.MathLibrary.Optimisation.Vector
{
    public static class BFGS_B
    {        /// <summary>
             /// Minimise a function and its derivative.
             /// </summary>
        public static OptimisationResult<VectorFuncEvaluation> Minimise(
            double[] startingPoint,
            double[] lowerBound,
            double[] upperBound,
            double gradientTolerance,
            Func<double[], double> func,
            Func<double[], double[]> gFunc,
            double tolerance = MathConstants.TolX,
            int maxIterations = 200)
        {
            int numDimensions = startingPoint.Length;

            // The current candidate minimum.
            double[] candidateMinimum = startingPoint;

            for (int index = 0; index < numDimensions; index++)
            {
                if (candidateMinimum[index] < lowerBound[index] || candidateMinimum[index] > upperBound[index])
                {
                    return OptimisationResult<VectorFuncEvaluation>.ErrorResult();
                }
            }

            double funcValue = func(candidateMinimum);
            double[] gradValue = gFunc(candidateMinimum);

            double[,] hessian = MatrixFunctions.Identity(numDimensions);
            var lineSearcher = CreateLineSearcher(numDimensions, candidateMinimum);


            return OptimisationResult<VectorFuncEvaluation>.ErrorResult();
        }

        private static ILineSearcher CreateLineSearcher(int numDimensions, double[] startingPoint)
        {
            double startingDotProduct = VectorFunctions.DotProduct(startingPoint, startingPoint);
            return new DefaultLineSearcher(MathConstants.STPMX * Math.Max(Math.Sqrt(startingDotProduct), (double)numDimensions));
        }

    }
}
