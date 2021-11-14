using System;

using Common.Structure.MathLibrary.Functions;

namespace Common.Structure.MathLibrary.ProbabilityDistributions
{
    /// <summary>
    /// The Gamma distribution
    /// </summary>
    public class GammaDistribution : IProbabilityDistribution
    {
        private readonly double Alpha;
        private readonly double Beta;
        private readonly double Fac;

        public GammaDistribution(double alpha, double beta)
        {
            if (alpha < 0.0 || beta <= 0.0)
            {
                // bad alpha and beta in gamma distribution.
                return;
            }

            Alpha = alpha;
            Beta = beta;
            Fac = alpha + Math.Log(beta) - BasicFunctions.LogGamma(alpha);
        }

        /// <inheritdoc/>
        public double Probabilitydensity(double x)
        {
            if (x <= 0.0)
            {
                return double.NaN;
            }
            return Math.Exp(-Beta * x * (Alpha - 1) * Math.Log(x) + Fac);
        }

        /// <inheritdoc/>
        public double Cdf(double x)
        {
            if (x <= 0.0)
            {
                return double.NaN;
            }

            return Gamma.GammaP(Alpha, Beta * x);
        }

        /// <inheritdoc/>
        public double InverseCdf(double p)
        {
            if (p < 0.0 || p >= 1)
            {
                return double.NaN;
            }

            return Gamma.InverseGammaP(p, Alpha) / Beta;
        }
    }
}
