using System.Collections.Generic;

using Common.Structure.MathLibrary.Matrices;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.Matrices
{
    [TestFixture]
    internal sealed class DoubleMatrixTests
    {
        private static IEnumerable<TestCaseData> IdentityData()
        {
            yield return new TestCaseData(1, MatrixTestHelper.ExampleMatrices.OneDIdentity);
            yield return new TestCaseData(2, MatrixTestHelper.ExampleMatrices.TwoDIdentity);
            yield return new TestCaseData(3, MatrixTestHelper.ExampleMatrices.ThreeDIdentity);
            yield return new TestCaseData(7, MatrixTestHelper.ExampleMatrices.SevenDIdentity);
        }

        [TestCaseSource(nameof(IdentityData))]
        public void IdentityCorrect(int n, string expectedMatrixIndex)
        {
            Assert.AreEqual(MatrixTestHelper.ExampleMatrices.Matrix(expectedMatrixIndex), DoubleMatrix.Identity(n));
        }

        private static IEnumerable<TestCaseData> TransposeData()
        {
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.OneDIdentity,
                new DoubleMatrix(new double[,] { { 1 } }));
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.TwoDIdentity,
                new DoubleMatrix(new double[,]
                { { 1, 0 },
                { 0, 1 } }));
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDIdentity,
                new DoubleMatrix(new double[,]
                { { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 } }));
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDSingleOffDiagonal,
                new DoubleMatrix(new double[,]
                { { 1, 0, 0 },
                { 0, 1, 0 },
                { 1, 0, 1 } }));
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDIntegerEntries,
                new DoubleMatrix(new double[,]
                { { 1, 4, 7 },
                { 2, 5, 7 },
                { 3, 6, 9 } }));
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDMixedEntries,
                new DoubleMatrix(new double[,]
                { { 7, 9, 4 },
                { 4.3, 2.2, 88 },
                { 3, -7.2, -2.3 } }));
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.FourDMixed,
                new DoubleMatrix(new double[,]
                { { 1, 4, 6.7, 1 },
                { 2.2, 3, 5, -1 },
                { 3, 6, -2.2, 0.2 },
                { -4.1, 8, 1, 45 }
                }));
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.FourDSymmetricNotPosDef,
                new DoubleMatrix(new double[,]
                { { 1, 4, 6.7, 4.1 },
                { 4, 3, 5, -1 },
                { 6.7, 5, 7, 0 },
                { 4.1, -1, 0, 9 } }));
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.SevenDIdentity,
                new DoubleMatrix(new double[,]
                { { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 } }));
        }

        [TestCaseSource(nameof(TransposeData))]
        public void TransposeCorrect(string expectedMatrixIndex, DoubleMatrix expectedMatrix)
        {
            double[,] matrix = MatrixTestHelper.ExampleMatrices.Matrix(expectedMatrixIndex);
            DoubleMatrix mat = new DoubleMatrix(matrix);
            Assert.AreEqual(expectedMatrix, mat.Transpose());
        }

        public static IEnumerable<TestCaseData> AddTestCases()
        {
            yield return new TestCaseData(
                new double[,] { { 1, 0 }, { 0, 1 } },
                new double[,] { { 0, 0 }, { 0, 0 } },
                new double[,] { { 1, 0 }, { 0, 1 } })
                .SetName("MatrixAdd-IdentityAddNothing");

            yield return new TestCaseData(
                new double[,] { { 1, 0, 0 }, { 0, 1, 0 } },
                new double[,] { { 0, 0, 0 }, { 0, 0, 0 } },
                new double[,] { { 1, 0, 0 }, { 0, 1, 0 } })
                .SetName("MatrixAdd-NonSquareAdd");

            yield return new TestCaseData(
                new double[,] { { 1, 0 }, { 0, 1 } },
                new double[,] { { 0, 2 }, { 0, 2 } },
                new double[,] { { 1, 2 }, { 0, 3 } })
                .SetName("MatrixAdd-IdentityAddValues");
        }

        [TestCaseSource(nameof(AddTestCases))]
        public void AddTests(double[,] firstMatrix, double[,] secondMatrix, double[,] expectedOutput)
        {
            double[,] add = DoubleMatrix.Add(firstMatrix, secondMatrix);
            Assert.AreEqual(expectedOutput, add);
        }

        [Test]
        public void XTXPlusICorrect([Values(1, 2, 3, 4, 5, 6, 7)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            double[,] actual = matrix.Matrix.XTXPlusI(1);
            Assertions.AreEqual(matrix.XTXPlusI, actual, 1e-3, "products wrong");
        }

        private static IEnumerable<TestCaseData> InverseData()
        {
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.OneDIdentity,
                new double[,] { { 1 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.TwoDIdentity,
                new double[,]
                { { 1, 0 },
                { 0, 1 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDIdentity,
                new double[,]
                { { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDSingleOffDiagonal,
                new double[,]
                { { 1, 0, -1 },
                { 0, 1, 0 },
                { 0, 0, 1 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDIntegerEntries,
                new double[,]
                { { -0.49999999999999956, -0.5, 0.5 },
                { -1.0000000000000007, 2, -1 },
                { 1.166666666666667, -1.1666666666666666666666, 0.5 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDMixedEntries,
                new double[,]
                { { 0.0936087, 0.0407905, -0.00559382 },
                { -0.00120634, -0.00418494, 0.0115272 },
                { 0.116642, -0.0891795, -0.00347008 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.FourDMixed,
                new double[,]
                { { -0.668029, 0.358001, 0.0539901, -0.125709 },
                { 0.87218, -0.407763, 0.0908859, 0.149937 },
                { -0.0365905, 0.155492, -0.0830216, -0.0291319 },
                { 0.0343895, -0.0177081, 0.00118889, 0.0284772 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.SevenDIdentity,
                new double[,]
                { { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 } });
        }

        [TestCaseSource(nameof(InverseData))]
        public void InverseCorrect(string expectedMatrixIndex, double[,] expectedInverse)
        {
            double[,] matrix = MatrixTestHelper.ExampleMatrices.Matrix(expectedMatrixIndex);
            Assertions.AreEqual(expectedInverse, matrix.Inverse(), 1e-6);
        }
    }
}
