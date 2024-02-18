using System;

namespace Effanville.Common.Structure.Tests.MathLibrary.RootFinding
{
    public sealed class TestData<T>
    {
        public double Lower
        {
            get;
            set;
        }
        public double Upper
        {
            get;
            set;
        }

        public Func<double, double> Func
        {
            get;
            set;
        }

        public int MaxIterations
        {
            get;
            set;
        }

        public double Tolerance
        {
            get;
            set;
        } = 1e-8;

        public T ExpectedResult
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }
    }
}
