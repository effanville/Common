using NUnit.Framework;
using StructureCommon.MathLibrary.ParameterEstimation;

namespace StructureCommon.Tests.MathLibrary.ParameterEstimation
{
    public class RidgeRegressionTests
    {
        [Test]
        public void AgreesWithLSE([Values(4, 5, 6)] int valuesIndex)
        {
            EstimatorValues estimatorValues = EstimatorValues.GetValues(valuesIndex);
            RidgeRegression estimator = new RidgeRegression(estimatorValues.data, estimatorValues.rhs);
            Assertions.AreEqual(estimatorValues.expectedEstimator, estimator.Estimator, 1e-5, "Expected Estimator not correct");
        }

        [Test]
        public void AgreesWithLSEZeroRegularisation([Values(4, 5, 6)] int valuesIndex)
        {
            EstimatorValues estimatorValues = EstimatorValues.GetValues(valuesIndex);
            RidgeRegression estimator = new RidgeRegression(estimatorValues.data, estimatorValues.rhs, 0);
            Assertions.AreEqual(estimatorValues.expectedEstimator, estimator.Estimator, 1e-5, "Expected Estimator not correct");
        }
    }
}
