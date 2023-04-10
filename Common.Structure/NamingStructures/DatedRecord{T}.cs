using System;
using System.Collections.Generic;

namespace Common.Structure.NamingStructures
{
    public sealed class DatedRecordComparisons
    {
        public static Comparison<DatedRecord<T>> ValueCompare<T>() where T : IComparable<T>
        {
            return (a, b) => b.Value.CompareTo(a.Value);
        }

        public static Comparison<DatedRecord<T>> DateCompare<T>()
        {
            return (a, b) => b.Date.CompareTo(a.Date);
        }

        public static Comparison<DatedRecord<T>> InverseDateCompare<T>()
        {
            return (a, b) => -1 * b.Date.CompareTo(a.Date);
        }
    }

    public sealed class DatedRecord<T>
    {
        private readonly string fRecordName;
        private readonly Func<T, T, T> fAggregation;

        public DateTime Date
        {
            get;
            private set;
        }

        public T Value
        {
            get;
            private set;
        }

        public DatedRecord(string recordName, DateTime start, T firstValue, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Date = start;
            Value = firstValue;
        }

        public void UpdateValue(T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
        }

        public override string ToString()
        {
            return $"{fRecordName}-{Date}-{Value}";
        }

        public IReadOnlyList<string> Values()
        {
            return new List<string>
                    {
                            Date.ToShortDateString(),
                            Value.ToString()
                    };
        }
    }
}
