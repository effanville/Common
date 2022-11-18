using Common.Structure.MathLibrary.Matrices;

namespace Common.Structure.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Contains Estimator methods.
    /// </summary>
    public static partial class Estimator
    {
        /// <summary>
        /// Contains LeastSquares estimation methods.
        /// </summary>
        public static class LeastSquares
        {
            /// <summary>
            /// The result of a Least Squares estimator.
            /// </summary>
            public sealed class LeastSquaresResult : Result
            {
                /// <summary>
                /// Construct an instance.
                /// </summary>
                public LeastSquaresResult(double[,] fitData,
                    double[] fitValues,
                    double[] sigma,
                    double[] estimator,
                    double[,] estimatorCovariance,
                    double estimatorFit)
                    : base(fitData, fitValues, sigma, estimator, estimatorCovariance, estimatorFit)
                {
                }

                /// <inheritdoc/>
                public override double Evaluate(double[] point)
                {
                    return EstimatorHelpers.Evaluate(Estimator, point);
                }
            }

            /// <summary>
            /// Perform the Least Squares fit of the data.
            /// </summary>
            public static LeastSquaresResult Fit(double[,] fitData, double[] fitValues)
            {
                double[] XTY = fitData.Transpose().PostMultiplyVector(fitValues);
                double[,] matrix = fitData.XTX().Inverse();
                double[] estimator = matrix.PostMultiplyVector(XTY);
                double residual = 0.0;
                for (int i = 0; i < fitData.GetLength(0); i++)
                {
                    double value = fitValues[i];
                    for (int j = 0; j < fitData.GetLength(1); j++)
                    {
                        value -= estimator[j] * fitData[i, j];
                    }

                    residual += value * value;
                }

                residual /= (fitValues.Length - estimator.Length);
                double[,] uncertainty = matrix.ScalarMult(residual);

                return new LeastSquaresResult(
                    fitData,
                    fitValues,
                    null,
                    estimator,
                    uncertainty,
                    residual);
            }
        }
    }
}
