using NUnit.Framework;

namespace Common.Structure.Tests
{
    public static class Assertions
    {
        /// <summary>
        /// Asserts that two matrices are equal up to the given tolerance.
        /// </summary>
        public static void AreEqual(double[,] expected, double[,] actual, double tol = 1e-8, string message = null)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            if ((expected == null && actual != null) || (actual == null && expected != null))
            {
                throw new AssertionException("One was null but other wasnt");
            }

            if (!expected.GetLength(0).Equals(actual.GetLength(0)))
            {
                throw new AssertionException($"Number of rows not the same. Expected {expected.GetLength(0)} but actually {actual.GetLength(0)}");
            }

            if (!expected.GetLength(1).Equals(actual.GetLength(1)))
            {
                throw new AssertionException($"Number of columns not the same. Expected {expected.GetLength(1)} but actually {actual.GetLength(1)}");
            }

            Assert.Multiple(() =>
            {
                for (int rowIndex = 0; rowIndex < expected.GetLength(0); rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < expected.GetLength(1); columnIndex++)
                    {
                        Assert.AreEqual(expected[rowIndex, columnIndex], actual[rowIndex, columnIndex], tol, $"{message}-row{rowIndex}-column{columnIndex}");
                    }
                }
            });
        }

        /// <summary>
        /// Asserts that two matrices are equal up to the given tolerance.
        /// </summary>
        public static void AreEqual(double[] expected, double[] actual, double tol = 1e-8, string message = null)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            if (!expected.GetLength(0).Equals(actual.GetLength(0)))
            {
                throw new AssertionException($"Number of rows not the same. Expected {expected.GetLength(0)} but actually {actual.GetLength(0)}");
            }

            Assert.Multiple(() =>
            {
                for (int rowIndex = 0; rowIndex < expected.GetLength(0); rowIndex++)
                {
                    Assert.AreEqual(expected[rowIndex], actual[rowIndex], tol, $"{message}-row{rowIndex}");
                }
            });
        }
    }
}
