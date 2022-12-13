using Common.Structure.MathLibrary.Matrices;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.Matrices
{
    [TestFixture]
    public sealed class LUDecompositionTests
    {
        [Test]
        public void LUDecompCorrect([Values(1, 2, 3, 4, 5, 6, 7, 8)] int expectedMatrixIndex)
        {
            TestMatrixValues matrix = MatrixTestHelper.GetMatrix(expectedMatrixIndex);
            LUDecomposition lUDecomposition = LUDecomposition.Generate(matrix.Matrix).Value;
            double[,] product = lUDecomposition.Lower.Multiply(lUDecomposition.Upper);

            Assertions.AreEqual(matrix.Matrix, product, 1e-3, "products wrong");

            Assertions.AreEqual(matrix.Upper, lUDecomposition.Upper, 1e-3, "Upper wrong");
            Assertions.AreEqual(matrix.Lower, lUDecomposition.Lower, 1e-3, "Lower wrong");
        }
    }
}
