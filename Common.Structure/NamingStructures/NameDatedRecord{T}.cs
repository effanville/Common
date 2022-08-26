﻿using System;
using System.Collections.Generic;

namespace Common.Structure.NamingStructures
{
    public sealed class NameDatedRecordComparisons
    {
        public static Comparison<NameDatedRecord<T>> ValueCompare<T>() where T : IComparable<T>
        {
            return (a, b) => b.Value.CompareTo(a.Value);
        }
    }

    public sealed class NameDatedRecord<T>
    {
        private readonly string fRecordName;
        private readonly Func<T, T, T> fAggregation;
        public Name Name
        {
            get;
            private set;
        }

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

        public NameDatedRecord(string recordName, Name name, DateTime start, T firstValue, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Name = name;
            Date = start;
            Value = firstValue;
        }

        public void UpdateValue(T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
        }

        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Date}-{Value}";
        }

        public IReadOnlyList<string> Values()
        {
            return new List<string>
                    {
                            Name.ToString(),
                            Date.ToShortDateString(),
                            Value.ToString()
                    };
        }
    }
}
