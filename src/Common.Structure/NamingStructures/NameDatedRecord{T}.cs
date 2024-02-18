using System;
using System.Collections.Generic;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// A record that contains a name and a date.
    /// </summary>
    public sealed class NameDatedRecord<T>
    {
        private readonly string fRecordName;
        private readonly Func<T, T, T> fAggregation;

        /// <summary>
        /// The name of the record.
        /// </summary>
        public Name Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The date of the record.
        /// </summary>
        public DateTime Date
        {
            get;
            private set;
        }

        /// <summary>
        /// The value to be recorded.
        /// </summary>
        public T Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public NameDatedRecord(string recordName, Name name, DateTime start, T firstValue, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Name = name;
            Date = start;
            Value = firstValue;
        }

        /// <summary>
        /// Update the value of the record.
        /// </summary>
        public void UpdateValue(T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Date}-{Value}";
        }

        /// <summary>
        /// Return the values in a list format.
        /// </summary>
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
