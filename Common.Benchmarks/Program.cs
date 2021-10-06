using System;
using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using Common.Structure.DataStructures;

namespace Common.Benchmarks
{

    public sealed class Program
    {
        [CsvExporter]
        [SimpleJob(launchCount: 1, warmupCount: 10, targetCount: 50)]
        public class TimeListBenchmarkTests
        {

            [ParamsSource(nameof(ValuesForA))]
            public TimeList Data
            {
                get;
                set;
            }

            public IEnumerable<TimeList> ValuesForA()
            {
                yield return TimeListTestData.GetTestTimeList("TenEntry");
                yield return TimeListTestData.GetTestTimeList("HundredEntry");
                yield return TimeListTestData.GetTestTimeList("ThousandEntry");
                yield return TimeListTestData.GetTestTimeList("TenThousandEntry");
            }

            public IEnumerable<DateTime> ValuesForB()
            {
                yield return new DateTime(2021, 9, 28);
            }

            [ParamsSource(nameof(ValuesForB))]
            public DateTime date
            {
                get; set;
            }

            [Benchmark]
            public DailyValuation TotalValueStart()
            {
                return Data.Value(Data[1].Day);
            }

            [Benchmark]
            public DailyValuation TotalValueMiddleDate()
            {
                return Data.Value(Data[(int)Math.Floor((double)(Data.Count() / 2))].Day);
            }

            [Benchmark]
            public DailyValuation TotalValueEnd()
            {
                return Data.Value(Data[Data.Count() - 2].Day);
            }
        }

        public static void Main(string[] args)
        {
            Summary[] summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
