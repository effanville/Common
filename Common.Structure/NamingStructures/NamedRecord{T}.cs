using System;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// A named record with a single value.
    /// </summary>
    public sealed class NamedRecord<T>
    {
        private readonly Func<T, T, T> fAggregation;
        private readonly string fRecordName;

        /// <summary>
        /// The name of the record.
        /// </summary>
        public Name Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The value of the record.
        /// </summary>
        public T Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public NamedRecord(string recordName, Name name, T value, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Update the value.
        /// </summary>
        public void UpdateValue(T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
        }

        /// <summary>
        /// Return the record name.
        /// </summary>
        public string RecordName()
        {
            return fRecordName;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Value}";
        }

        /// <summary>
        /// Return the values in an array.
        /// </summary>
        public string[] ArrayValues()
        {
            return new string[] { Name.ToString(), Value.ToString() };
        }
    }
}
