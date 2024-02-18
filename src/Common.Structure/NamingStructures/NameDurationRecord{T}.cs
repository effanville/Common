using System;

namespace Effanville.Common.Structure.NamingStructures
{
    /// <summary>
    /// Contains a record associated to a duration for a named entity.
    /// <para/>
    /// An example is the number of times a specific book has been loaned 
    /// in a period.
    /// </summary>
    public sealed class NameDurationRecord<T>
    {
        private readonly string fRecordName;
        private readonly Func<T, T, T> fAggregation;

        /// <summary>
        /// The name associated to this record.
        /// </summary>
        public Name Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The start date of the duration for this record.
        /// </summary>
        public DateTime Start
        {
            get;
            private set;
        }

        /// <summary>
        /// The end date of the duration of this record.
        /// </summary>
        public DateTime End
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
        /// Construct an instance of a <see cref="NameDurationRecord{T}"/>.
        /// </summary>
        public NameDurationRecord(string recordName, Name name, DateTime start, T firstValue, Func<T, T, T> aggregation)
        {
            fRecordName = recordName;
            fAggregation = aggregation;
            Name = name;
            Start = start;
            End = start;
            Value = firstValue;
        }

        /// <summary>
        /// Update the value stored and alter dates to include this new time.
        /// </summary>
        public void UpdateValue(DateTime newDate, T additionalValue)
        {
            Value = fAggregation(Value, additionalValue);
            End = End < newDate ? newDate : End;
            Start = Start > newDate ? newDate : Start;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{fRecordName}-{Name}-{Start}-{End}-{Value}";
        }

        /// <summary>
        /// Returns an array with the values of the record.
        /// </summary>
        public string[] ArrayOfValues()
        {
            return new string[]
            {
                Name.ToString(),
                Start.ToShortDateString(),
                End.ToShortDateString(),
                Value.ToString()
            };
        }
    }
}
