using Common.Structure.MathLibrary.ParameterEstimation;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.ParameterEstimation
{
    public class LassoRegressionTests
    {
        [Test]
        public void AgreesWithLSE([Values(4, 5, 6)] int valuesIndex)
        {
            EstimatorValues estimatorValues = EstimatorValues.GetValues(valuesIndex);
            var lassoResult = Estimator.LassoRegression.Fit(estimatorValues.data, estimatorValues.rhs);
            Assertions.AreEqual(estimatorValues.expectedEstimator, lassoResult.Estimator, 5e-2, "Expected Estimator not correct");
        }

        [Test]
        public void AgreesWithLSEZeroLambda([Values(4, 5, 6)] int valuesIndex)
        {
            EstimatorValues estimatorValues = EstimatorValues.GetValues(valuesIndex);
            var lassoResult = Estimator.LassoRegression.Fit(estimatorValues.data, estimatorValues.rhs, 0.01);
            Assertions.AreEqual(estimatorValues.expectedEstimator, lassoResult.Estimator, 1e-3, "Expected Estimator not correct");
        }
    }
}
