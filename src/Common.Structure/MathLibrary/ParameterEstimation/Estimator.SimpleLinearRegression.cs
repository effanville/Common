using System;

using Common.Structure.MathLibrary.Functions;

namespace Common.Structure.MathLibrary.ParameterEstimation
{
    public static partial class Estimator
    {
        /// <summary>
        /// Object for fitting a straight line y = a + b x to a set of points (x_i,y_i) with or without error 
        /// terms sigma_i in the measurements.
        /// Taken from Numerical Recipes in C 3rd edition.
        /// </summary>
        public static class SimpleLinearRegression
        {
            /// <summary>
            /// The result of a simple linear regression.
            /// </summary>
            public sealed class SimpleLinearResult : Result
            {
                /// <summary>
                /// The Chi Squared statistic for the result.
                /// </summary>
                public double ChiSquared
                {
                    get;
                }

                /// <summary>
                /// Construct an instance of a <see cref="SimpleLinearResult"/>
                /// </summary>
                public SimpleLinearResult(
                    double[,] fitData,
                    double[] fitValues,
                    double[] sigma,
                    double[] estimator,
                    double[,] estimatorCovariance,
                    double estimatorFit,
                    double chiSquared)
                    : base(fitData,
                          fitValues,
                          sigma,
                          estimator,
                          estimatorCovariance,
                          estimatorFit)
                {
                    ChiSquared = chiSquared;
                }

                /// <inheritdoc/>
                public override double Evaluate(double[] point)
                {
                    if (point.Length != 1)
                    {
                        return double.NaN;
                    }

                    return Estimator[0] + point[0] * Estimator[1];
                }
            }

            /// <summary>
            /// Perform the Fit of the regression.
            /// </summary>
            public static SimpleLinearResult Fit(double[,] fitData, double[] fitValues)
            {
                double ss, sx = 0.0, sy = 0.0;
                double t;
                double st2 = 0;
                double b = 0.0;
                for (int i = 0; i < fitValues.Length; i++)
                {
                    sx += fitData[i, 0];
                    sy += fitValues[i];
                }
                ss = fitValues.Length;
                double sxoss = sx / ss;

                for (int i = 0; i < fitValues.Length; i++)
                {
                    t = fitData[i, 0] - sxoss;
                    st2 += t * t;
                    b += t * fitValues[i];
                }

                b /= st2;

                double a = (sy - sx * b) / ss;
                double siga = Math.Sqrt((1 + sx * sx / (ss * st2)) / ss);
                double sigb = Math.Sqrt(1 / st2);
                double chi2 = 0;
                for (int i = 0; i < fitValues.Length; i++)
                {
                    chi2 += Math.Pow(fitValues[i] - a - b * fitData[i, 0], 2.0);
                }

                double sigdat = 1.0;
                if (fitValues.Length > 2)
                {
                    sigdat = Math.Sqrt(chi2 / (fitValues.Length - 2));
                }

                siga *= sigdat;
                sigb *= sigdat;
                double cov = (-1) * sx / (ss * st2);
                double[] estimator = new double[] { a, b };
                double chiSquared = chi2;
                double estimatorFit = 1.0;
                double[,] estimatorCovariance = new double[,] { { siga, cov }, { cov, sigb } };

                return new SimpleLinearResult(
                    fitData,
                    fitValues,
                    null,
                    estimator,
                    estimatorCovariance,
                    estimatorFit,
                    chiSquared);
            }

            /// <inheritdoc/>
            public static SimpleLinearResult Fit(double[,] fitData, double[] fitValues, double[] sigma)
            {
                int i;
                double ss = 0;
                double sx = 0, sy = 0, st2 = 0;
                double t, wt, sxoss;
                double b = 0.0;
                for (i = 0; i < fitValues.Length; i++)
                {
                    wt = 1 / Math.Pow(sigma[i], 2.0);
                    ss += wt;
                    sx += fitData[i, 0] * wt;
                    sy = fitValues[i] * wt;
                }

                sxoss = sx / ss;
                for (i = 0; i < fitValues.Length; i++)
                {
                    t = (fitData[i, 0] - sxoss) / sigma[i];
                    st2 += t * 2;
                    b += t * fitValues[i] / sigma[i];
                }

                b /= st2;
                double a = (sy - sx * b) / ss;
                double siga = Math.Sqrt((1 + sx * sx / (ss * st2)) / ss);
                double sigb = Math.Sqrt(1 / st2);

                double chi2 = 0;

                for (i = 0; i < fitValues.Length; i++)
                {
                    chi2 += Math.Pow(((fitValues[i] - a - b * fitData[i, 0]) / sigma[i]), 2);
                }

                double cov = (-1) * sx / (ss * st2);
                double q = 0;
                if (fitValues.Length > 2)
                {
                    q = Gamma.GammaQ(0.5 * (fitValues.Length - 2), 0.5 * chi2);
                }

                double[] estimator = new double[] { a, b };
                double chiSquared = chi2;
                double estimatorFit = q;
                double[,] estimatorCovariance = new double[,] { { siga, cov }, { cov, sigb } };
                return new SimpleLinearResult(
                    fitData,
                    fitValues,
                    null,
                    estimator,
                    estimatorCovariance,
                    estimatorFit,
                    chiSquared);
            }
        }
    }
}
