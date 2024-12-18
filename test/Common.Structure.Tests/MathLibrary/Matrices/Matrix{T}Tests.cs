﻿using System.Collections.Generic;

using Effanville.Common.Structure.MathLibrary.Matrices;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.Matrices
{
    public class Matrix_T_Tests
    {
        private static IEnumerable<TestCaseData> TransposeData()
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
                { { 1, 0, 0 },
                { 0, 1, 0 },
                { 1, 0, 1 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDIntegerEntries,
                new double[,]
                { { 1, 4, 7 },
                { 2, 5, 7 },
                { 3, 6, 9 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.ThreeDMixedEntries,
                new double[,]
                { { 7, 9, 4 },
                { 4.3, 2.2, 88 },
                { 3, -7.2, -2.3 } });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.FourDMixed,
                new double[,]
                { { 1, 4, 6.7, 1 },
                { 2.2, 3, 5, -1 },
                { 3, 6, -2.2, 0.2 },
                { -4.1, 8, 1, 45 }
                });
            yield return new TestCaseData(
                MatrixTestHelper.ExampleMatrices.FourDSymmetricNotPosDef,
                new double[,]
                { { 1, 4, 6.7, 4.1 },
                { 4, 3, 5, -1 },
                { 6.7, 5, 7, 0 },
                { 4.1, -1, 0, 9 } });
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

        [TestCaseSource(nameof(TransposeData))]
        public void TransposeCorrect(string expectedMatrixIndex, double[,] expectedMatrix)
        {
            double[,] matrix = MatrixTestHelper.ExampleMatrices.Matrix(expectedMatrixIndex);
            Assert.That(matrix.Transpose(), Is.EqualTo(expectedMatrix));
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(3, 4)]
        [TestCase(3, 5)]
        [TestCase(3, 6)]
        [TestCase(4, 3)]
        [TestCase(4, 4)]
        [TestCase(4, 5)]
        [TestCase(4, 6)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        [TestCase(5, 5)]
        [TestCase(5, 6)]
        [TestCase(6, 3)]
        [TestCase(6, 4)]
        [TestCase(6, 5)]
        [TestCase(6, 6)]
        [TestCase(7, 7)]
        public void MultiplyOK(int matrixIndex, int matrix2Index)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(matrixIndex);
            TestMatrixValues vector = MatrixTestHelper.GetMatrix(matrix2Index);
            double[,] expected = MatrixTestHelper.GetExpectedMatrixProduct(matrixIndex, matrix2Index);
            Assert.That(matrix.Matrix.Multiply(vector.Matrix), Is.EqualTo(expected));
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(3, 5)]
        [TestCase(3, 6)]
        [TestCase(3, 7)]
        [TestCase(4, 4)]
        [TestCase(4, 5)]
        [TestCase(4, 6)]
        [TestCase(4, 7)]
        [TestCase(5, 4)]
        [TestCase(5, 5)]
        [TestCase(5, 6)]
        [TestCase(5, 7)]
        [TestCase(6, 4)]
        [TestCase(6, 5)]
        [TestCase(6, 6)]
        [TestCase(6, 7)]
        public void VectorMatrixMultiplyOK(int matrixIndex, int vectorIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(matrixIndex);
            double[] vector = MatrixTestHelper.GetVector(vectorIndex);
            double[] expected = MatrixTestHelper.GetExpectedProduct(matrixIndex, vectorIndex);
            Assert.That(matrix.Matrix.PostMultiplyVector(vector), Is.EqualTo(expected));
        }

        [Test]
        public void ComputeXTXOK([Values(1, 2, 3, 4, 5, 6, 7)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            Assert.That(matrix.Matrix.XTX(), Is.EqualTo(matrix.XTX));
        }

        [Test]
        public void ComputeIsSymmetric([Values(1, 2, 3, 4, 5, 6, 7, 8, 9)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            Assert.That(Matrix<double>.IsSymmetric(matrix.Matrix), Is.EqualTo(matrix.IsSymmetric));
        }
    }
}
