using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureCommon.DataStructures
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    /// <remarks>This list is sorted, with oldest value the first and latest the last.</remarks>
    public partial class TimeList : ITimeList
    {
        /// <summary>
        /// Event that controls when data is edited.
        /// </summary>
        public event EventHandler DataEdit;

        internal void OnDataEdit(object edited)
        {
            DataEdit?.Invoke(edited, new EventArgs());
        }

        /// <summary>
        /// Collection of data within the TimeList.
        /// </summary>
        private List<DailyValuation> fValues;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public List<DailyValuation> Values
        {
            get
            {
                return fValues;
            }
            set
            {
                fValues = value;
            }
        }

        /// <inheritdoc/>
        public DailyValuation this[int index]
        {
            get
            {
                return new DailyValuation(fValues[index]);
            }
        }

        /// <summary>
        /// Constructor adding values.
        /// </summary>
        /// <remarks>For testing only.</remarks>
        public TimeList(List<DailyValuation> values)
        {
            fValues = values;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TimeList()
        {
            fValues = new List<DailyValuation>();
        }

        /// <inheritdoc/>
        public bool Any()
        {
            return fValues != null && fValues.Any();
        }

        /// <inheritdoc/>
        public int Count()
        {
            return fValues.Count;
        }

        /// <inheritdoc/>
        public void CleanValues()
        {
            if (fValues.Count <= 1)
            {
                return;
            }

            var lastValue = fValues[0];
            for (int valueIndex = 1; valueIndex < fValues.Count; ++valueIndex)
            {
                if (fValues[valueIndex].Value.Equals(lastValue.Value))
                {
                    fValues.RemoveAt(valueIndex);
                    --valueIndex;
                }
                else
                {
                    lastValue = fValues[valueIndex];
                }
            }
        }
    }
}
