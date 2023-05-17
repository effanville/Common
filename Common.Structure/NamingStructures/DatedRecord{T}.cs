using System;
using System.Collections.Generic;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// A record that contains a date and a value.
    /// </summary>]
    public sealed class DatedRecord<T>
    {
        private readonly string fRecordName;
        private readonly Func<T, T, T> fAggregation;

        /// <summary>
        /// The date to record.
        /// </summary>
        public DateTime Date
        {
            get;
            private set;
        }

        /// <summary>
        /// The value to record.
        /// </summary>
        public T Value
        {
            get;
            private set;
        }


        /// <summary>
        /// Construct an instance.
        /// </summary>
        public DatedRecord(string recordName, DateTime start, T firstValue, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Date = start;
            Value = firstValue;
        }

        /// <summary>
        /// Update the value.
        /// </summary>
        public void UpdateValue(T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{fRecordName}-{Date}-{Value}";
        }

        /// <summary>
        /// Return the data in a list format.
        /// </summary>
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
