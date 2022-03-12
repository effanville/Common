using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using Common.Structure.MathLibrary.Optimisation.Vector;
using Common.Structure.Tests.MathLibrary.Optimisation.Vector;

namespace Common.Benchmarks
{

    public sealed class Program
    {
        [MemoryDiagnoser]
        [JsonExporter]
        [SimpleJob(launchCount: 1, warmupCount: 10, targetCount: 50)]
        public class TimeListBenchmarkTests
        {

            [ParamsSource(nameof(ValuesForA))]
            public double[] Data
            {
                get;
                set;
            }

            public IEnumerable<double[]> ValuesForA()
            {
                yield return new double[] { 2, 4 };
                yield return new double[] { -5, -5 };
                yield return new double[] { 3.12, 2.44 };
                yield return new double[] { -3.0, -3.0 };
            }

            [Benchmark]
            public void CalcMin_Himmelblau()
            {
                var function = new HimmelblauFunction();
                var min = BFGS.Minimise(
                    Data,
                    gradientTolerance: 1e-5,
                    point => function.Value(point),
                    point => function.Gradient(point),
                    tolerance: 1e-5,
                    maxIterations: 1000);
            }
        }

        public static void Main(string[] args)
        {
            Summary[] summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
