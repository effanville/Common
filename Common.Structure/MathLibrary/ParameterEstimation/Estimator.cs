namespace Common.Structure.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Contains all parameter estimation routines.
    /// </summary>
    public static partial class Estimator
    {
        /// <summary>
        /// The type of the Estimator.
        /// </summary>
        public enum Type
        {
            SimpleLinearRegression,
            LeastSquares,
            LassoRegression,
            RidgeRegression
        }

        /// <summary>
        /// The result of the parameter estimation.
        /// </summary>
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

            /// <summary>
            /// Construct an instance.
            /// </summary>
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

        /// <summary>
        /// Routine to estimate parameters based upon the data and values, with the type of estimator to use.
        /// This calculates parameters p from Ap = y where A is the fitData and y are the fitValues.
        /// </summary>
        /// <param name="estimatorType">The type of estimator to use.</param>
        /// <param name="fitData">The data to use to fit the parameters.</param>
        /// <param name="fitValues">The values to use to fit.</param>
        /// <returns>A see <see cref="Result"/> containing the output of the estimation.</returns>
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
        /// <summary>
        /// Routine to estimate parameters based upon the data and values, with the type of estimator to use.
        /// This calculates parameters p from Ap = y where A is the fitData and y are the fitValues.
        /// </summary>
        /// <param name="estimatorType">The type of estimator to use.</param>
        /// <param name="fitData">The data to use to fit the parameters.</param>
        /// <param name="fitValues">The values to use to fit.</param>
        /// <param name="sigma">The measurement uncertainty in the fitValues.</param>
        /// <returns>A see <see cref="Result"/> containing the output of the estimation.</returns>
        public static Result Fit(Type estimatorType, double[,] fitData, double[] fitValues, double[] sigma)
        {
            switch (estimatorType)
            {
                case Type.SimpleLinearRegression:
                    return SimpleLinearRegression.Fit(fitData, fitValues, sigma);
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
