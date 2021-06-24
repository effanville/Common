using NUnit.Framework;
using Common.Structure.MathLibrary.ParameterEstimation;

namespace Common.Structure.Tests.MathLibrary.ParameterEstimation
{
    public class LassoRegressionTests
    {
        [Test]
        public void AgreesWithLSE([Values(4, 5, 6)] int valuesIndex)
        {
            EstimatorValues estimatorValues = EstimatorValues.GetValues(valuesIndex);
            LassoRegression estimator = new LassoRegression(estimatorValues.data, estimatorValues.rhs);
            Assertions.AreEqual(estimatorValues.expectedEstimator, estimator.Estimator, 5e-2, "Expected Estimator not correct");
        }

        [Test]
        public void AgreesWithLSEZeroLambda([Values(4, 5, 6)] int valuesIndex)
        {
            EstimatorValues estimatorValues = EstimatorValues.GetValues(valuesIndex);
            LassoRegression estimator = new LassoRegression(estimatorValues.data, estimatorValues.rhs, 0.01);
            Assertions.AreEqual(estimatorValues.expectedEstimator, estimator.Estimator, 1e-3, "Expected Estimator not correct");
        }
    }
}
