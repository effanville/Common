using Common.Structure.MathLibrary.ParameterEstimation;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.ParameterEstimation
{
    public class RidgeRegressionTests
    {
        [Test]
        public void AgreesWithLSE([Values(4, 5, 6)] int valuesIndex)
        {
            EstimatorValues estimatorValues = EstimatorValues.GetValues(valuesIndex);
            var ridgeResult = Estimator.RidgeRegression.Fit(estimatorValues.data, estimatorValues.rhs);
            Assertions.AreEqual(estimatorValues.expectedEstimator, ridgeResult.Estimator, 1e-5, "Expected Estimator not correct");
        }

        [Test]
        public void AgreesWithLSEZeroRegularisation([Values(4, 5, 6)] int valuesIndex)
        {
            EstimatorValues estimatorValues = EstimatorValues.GetValues(valuesIndex);
            var ridgeResult = Estimator.RidgeRegression.Fit(estimatorValues.data, estimatorValues.rhs, 0);
            Assertions.AreEqual(estimatorValues.expectedEstimator, ridgeResult.Estimator, 1e-5, "Expected Estimator not correct");
        }
    }
}
