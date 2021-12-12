using System.Collections.Generic;

using Common.Structure.MathLibrary;
using Common.Structure.MathLibrary.Matrices;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.Matrices
{
    [TestFixture]
    public sealed class CholeskyDecompositionTests
    {
        private static IEnumerable<TestCaseData> TransposeData()
        {
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.OneDIdentity,
                null,
                new double[,] { { 1 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.TwoDIdentity,
                null,
                new double[,]
                { { 1, 0 },
                { 0, 1 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDIdentity,
                null,
                new double[,]
                { { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDSingleOffDiagonal,
                "Matrix is not symmetric",
                null);
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDSymmetric,
                null,
                new double[,]
                { { 2.44948974, 0, 0 },
                { 6.12372435, 4.1833001, 0 },
                { 22.4536559, 20.9165006, 6.1101009 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDIntegerEntries,
                "Matrix is not symmetric",
                null);
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDMixedEntries,
                "Matrix is not symmetric",
                null);
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.FourDSymmetricNotPosDef,
                "Cannot compute Cholesky decomposition. Sum for row 1 and column 1 is negative.",
                null);
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.FourDSymmetric,
                null,
                new double[,]
                { { 2, 0, 0, 0 },
                { 3, 3, 0, 0 },
                { 5, 4, 3, 0 },
                { 1, 6, 5, 7 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.SevenDIdentity,
                null,
                new double[,]
                { { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 } });
        }

        [TestCaseSource(nameof(TransposeData))]
        public void CholeskyDecompCorrect(string matrixName, string expectedErrorMessage, double[,] expectedLowerDecomp)
        {
            double[,] matrix = MatrixTestHelper.ExampleMatrices.Matrix(matrixName);
            Result<CholeskyDecomposition> lUDecomposition = CholeskyDecomposition.Generate(matrix);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedErrorMessage, lUDecomposition.Error);

                var value = lUDecomposition.Value;
                if (value != null)
                {
                    double[,] product = value.LowerDecomp.Multiply(value.LowerTranspose());
                    Assertions.AreEqual(matrix, product, 1e-8, "products wrong");
                }

                Assertions.AreEqual(expectedLowerDecomp, value?.LowerDecomp, 1e-7, "Lower wrong");
            });
        }
    }
}
