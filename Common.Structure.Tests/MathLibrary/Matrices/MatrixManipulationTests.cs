using NUnit.Framework;
using Common.Structure.MathLibrary.Matrices;

namespace Common.Structure.Tests.MathLibrary.Matrices
{
    public class MatrixManipulationTests
    {
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(7, 7)]
        public void IdentityCorrect(int n, int expectedMatrixIndex)
        {
            Assert.AreEqual(MatrixTestHelper.GetMatrix(expectedMatrixIndex).Matrix, MatrixFunctions.Identity(n));
        }

        [Test]
        public void TransposeCorrect([Values(1, 2, 3, 4, 5, 6, 7)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            Assert.AreEqual(matrix.Transpose, matrix.Matrix.Transpose());
        }

        [Test]
        public void LUDecompCorrect([Values(1, 2, 3, 4, 5, 6, 7, 8)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            LUDecomposition lUDecomposition = new LUDecomposition(matrix.Matrix);
            double[,] product = lUDecomposition.LowerDecomp.Multiply(lUDecomposition.UpperDecomp);
            Assertions.AreEqual(matrix.Matrix, product, 1e-3, "products wrong");
            Assert.AreEqual(lUDecomposition.Invertible, true);
            Assertions.AreEqual(matrix.Upper, lUDecomposition.UpperDecomp, 1e-3, "Upper wrong");
            Assertions.AreEqual(matrix.Lower, lUDecomposition.LowerDecomp, 1e-3, "Lower wrong");
        }

        [Test]
        public void XTXPlusICorrect([Values(1, 2, 3, 4, 5, 6, 7)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            var actual = matrix.Matrix.XTXPlusI(1);
            Assertions.AreEqual(matrix.XTXPlusI, actual, 1e-3, "products wrong");
        }

        [Test]
        public void InverseCorrect([Values(1, 2, 3, 4, 5, 6, 7, 8)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            Assertions.AreEqual(matrix.Inverse, matrix.Matrix.Inverse(), 1e-6);
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
            Assert.AreEqual(expected, matrix.Matrix.Multiply(vector.Matrix));
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
            Assert.AreEqual(expected, matrix.Matrix.PostMultiplyVector(vector));
        }

        [Test]
        public void ComputeXTXOK([Values(1, 2, 3, 4, 5, 6, 7)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            Assert.AreEqual(matrix.XTX, matrix.Matrix.XTX());
        }
    }
}
