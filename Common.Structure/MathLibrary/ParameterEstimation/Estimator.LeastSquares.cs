using Common.Structure.MathLibrary.Matrices;

namespace Common.Structure.MathLibrary.ParameterEstimation
{
    public static partial class Estimator
    {
        public static class LeastSquares
        {
            public sealed class LeastSquaresResult : Result
            {
                public LeastSquaresResult(double[,] fitData,
                    double[] fitValues,
                    double[] sigma,
                    double[] estimator,
                    double[,] estimatorCovariance,
                    double estimatorFit)
                    : base(fitData, fitValues, sigma, estimator, estimatorCovariance, estimatorFit)
                {
                }

                public override double Evaluate(double[] point)
                {
                    return EstimatorHelpers.Evaluate(Estimator, point);
                }
            }

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
