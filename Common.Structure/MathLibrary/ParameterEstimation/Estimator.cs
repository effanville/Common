namespace Common.Structure.MathLibrary.ParameterEstimation
{
    public static partial class Estimator
    {
        public enum Type
        {
            SimpleLinearRegression,
            LeastSquares,
            LassoRegression,
            RidgeRegression
        }

        public abstract class Result
        {
            /// <summary>
            /// The fit data used to fit the parameters.
            /// </summary>
            public double[,] FitData
            {
                get;
            }

            /// <summary>
            /// The values data used to fit the parameters
            /// </summary>
            public double[] FitValues
            {
                get;
            }

            /// <summary>
            /// The measurement uncertainty in the Points.
            /// </summary>
            public double[] Sigma
            {
                get;
            }

            /// <summary>
            /// Returns the values one has estimated.
            /// </summary>
            public double[] Estimator
            {
                get;
            }

            /// <summary>
            /// The covariance matrix of the estimator.
            /// </summary>
            public double[,] EstimatorCovariance
            {
                get;
            }

            /// <summary>
            /// The quality of the fit given by the estimator.
            /// </summary>
            public double EstimatorFit
            {
                get;
            }

            public Result(double[,] fitData, double[] fitValues, double[] sigma, double[] estimator, double[,] estimatorCovariance, double estimatorFit)
            {
                FitData = fitData;
                FitValues = fitValues;
                Sigma = sigma;
                Estimator = estimator;
                EstimatorCovariance = estimatorCovariance;
                EstimatorFit = estimatorFit;
            }

            /// <summary>
            /// Evaluates the point using the estimator calculated.
            /// </summary>
            /// <param name="point">The value at which to evaluate at.</param>
            public abstract double Evaluate(double[] point);
        }

        public static Result Fit(Type estimatorType, double[,] fitData, double[] fitValues)
        {
            switch (estimatorType)
            {
                case Type.SimpleLinearRegression:
                    return SimpleLinearRegression.Fit(fitData, fitValues);
                case Type.LeastSquares:
                    return LeastSquares.Fit(fitData, fitValues);
                case Type.LassoRegression:
                    return LassoRegression.Fit(fitData, fitValues);
                case Type.RidgeRegression:
                    return RidgeRegression.Fit(fitData, fitValues);
                default:
                    return null;
            }
        }
    }
}
