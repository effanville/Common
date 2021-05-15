using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StructureCommon.DataStructures
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    /// <remarks>This list is sorted, with oldest value the first and latest the last.</remarks>
    public partial class TimeList : ITimeList, IEquatable<TimeList>, IXmlSerializable
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
        internal TimeList(List<DailyValuation> values)
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

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        private const string XmlBaseName = "Values";

        /// <inheritdoc/>
        public virtual void ReadXml(XmlReader reader)
        {
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement(XmlBaseName);

            if (!isEmpty)
            {
                while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
                {
                    var valuation = new DailyValuation();
                    valuation.ReadXml(reader);
                    fValues.Add(valuation);
                    _ = reader.MoveToContent();
                }

                if (reader.NodeType != XmlNodeType.None)
                {
                    reader.ReadEndElement();
                }
            }
        }

        /// <inheritdoc/>
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(XmlBaseName);
            foreach (var value in fValues)
            {
                value.WriteXml(writer);
            }

            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        public override bool Equals(object other)
        {
            if (other is TimeList otherTimeList)
            {
                return Equals(otherTimeList);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            foreach (var value in fValues)
            {
                hashCode = 23 * hashCode + value.GetHashCode();
            }

            return hashCode;
        }

        /// <inheritdoc/>
        public bool Equals(TimeList other)
        {
            int count = Count();
            if (count != other.Count())
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                var value = fValues[i];
                if (!value.Equals(other[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
