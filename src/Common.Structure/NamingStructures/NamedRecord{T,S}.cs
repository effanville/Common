using System;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// A named record with two values.
    /// </summary>
    public sealed class NamedRecord<T, S>
    {
        private readonly Func<T, T, T> fTAggregation;
        private readonly Func<S, S, S> fSAggregation;
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
        /// The primary value to record.
        /// </summary>
        public T Value
        {
            get;
            private set;
        }

        /// <summary>
        /// The secondary value to record.
        /// </summary>
        public S SecondValue
        {
            get;
            private set;
        }


        /// <summary>
        /// Construct an instance.
        /// </summary>
        public NamedRecord(
            string recordName,
            Name name,
            T value,
            S secondValue,
            Func<T, T, T> TAggregation,
            Func<S, S, S> SAggregation)
        {
            fRecordName = recordName;
            fTAggregation = TAggregation;
            fSAggregation = SAggregation;
            Name = name;
            Value = value;
            SecondValue = secondValue;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public NamedRecord(
            string recordName,
            Name name,
            T value,
            S secondValue)
        {
            fRecordName = recordName;
            Name = name;
            Value = value;
            SecondValue = secondValue;
        }

        /// <summary>
        /// Update the values.
        /// </summary>
        public void UpdateValue(T additionalTValue, S additionalSValue)
        {
            if (fTAggregation != null)
            {
                Value = fTAggregation(Value, additionalTValue);
            }
            if (fSAggregation != null)
            {
                SecondValue = fSAggregation(SecondValue, additionalSValue);
            }
        }

        /// <summary>
        /// Return the name of the record.
        /// </summary>
        public string RecordName()
        {
            return fRecordName;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Value}-{SecondValue}";
        }
    }
}
