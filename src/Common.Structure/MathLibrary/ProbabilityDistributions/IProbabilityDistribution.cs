namespace Common.Structure.MathLibrary.ProbabilityDistributions
{
    internal interface IProbabilityDistribution
    {
        /// <summary>
        /// Returns the probability density function at the point x.
        /// </summary>
        double Probabilitydensity(double x);

        /// <summary>
        /// Returns the cumulative distribution function at the point x.
        /// </summary>
        /// <param name="x">The value to calculate the Cdf at.</param>
        /// <returns>The probability of the value less than <paramref name="x"/></returns>
        double Cdf(double x);

        /// <summary>
        /// Returns the inverse cumulative distribution function.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        double InverseCdf(double p);
    }
}
