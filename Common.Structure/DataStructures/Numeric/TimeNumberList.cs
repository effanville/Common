using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Structure.DataStructures.Numeric
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    /// <remarks>This list is sorted, with oldest value the first and latest the last.</remarks>
    public partial class TimeNumberList : IEquatable<TimeNumberList>, IXmlSerializable
    {
        private readonly object valuesLock = new object();

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
        private List<DailyNumeric> fValues;

        /// <summary>
        /// Get a copy of the currently held data in the <see cref="TimeNumberList"/>
        /// </summary>
        /// <returns></returns>
        public List<DailyNumeric> Values()
        {
            lock (valuesLock)
            {
                return fValues.ToList();
            }
        }

        /// <inheritdoc/>
        public DailyNumeric this[int index]
        {
            get => new DailyNumeric(fValues[index]);
        }

        /// <summary>
        /// Returns a copy of the list.
        /// </summary>
        public TimeNumberList Copy()
        {
            return new TimeNumberList(Values());
        }

        /// <summary>
        /// Constructor adding values.
        /// </summary>
        /// <remarks>For testing only.</remarks>
        internal TimeNumberList(List<DailyNumeric> values)
        {
            fValues = values;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TimeNumberList()
        {
            fValues = new List<DailyNumeric>();
        }

        /// <inheritdoc/>
        public bool Any()
        {
            return Any(Values());
        }

        private static bool Any(List<DailyNumeric> values)
        {
            return values.Any();
        }

        /// <inheritdoc/>
        public int Count()
        {
            return Count(Values());
        }

        private static int Count(List<DailyNumeric> values)
        {
            return values.Count;
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
            bool isInnerEmpty = false;
            bool isEmpty = reader.IsEmptyElement;

            reader.ReadStartElement();

            // if the timelist is part of a larger class, then the data is stored in an
            // extra node.
            bool partOfClass = reader.AttributeCount > 0 || reader.LocalName == XmlBaseName;
            if (partOfClass)
            {
                isInnerEmpty = reader.IsEmptyElement;
                reader.ReadStartElement(XmlBaseName);
            }

            if (!isEmpty)
            {
                lock (valuesLock)
                {
                    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
                    {
                        var valuation = new DailyNumeric();
                        valuation.ReadXml(reader);
                        fValues.Add(valuation);
                        _ = reader.MoveToContent();
                    }
                }
                if (reader.NodeType != XmlNodeType.None)
                {
                    if (partOfClass && !isInnerEmpty)
                    {
                        reader.ReadEndElement();
                    }

                    reader.ReadEndElement();
                }
            }
        }

        /// <inheritdoc/>
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(XmlBaseName);

            lock (valuesLock)
            {
                foreach (var value in fValues)
                {
                    value.WriteXml(writer);
                }
            }
            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        public override bool Equals(object other)
        {
            if (other is TimeNumberList otherTimeList)
            {
                return Equals(otherTimeList);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;

            lock (valuesLock)
            {
                foreach (var value in fValues)
                {
                    hashCode = 23 * hashCode + value.GetHashCode();
                }
            }

            return hashCode;
        }

        /// <inheritdoc/>
        public bool Equals(TimeNumberList other)
        {
            lock (valuesLock)
            {
                int count = fValues.Count;
                var otherData = other.Values();
                if (count != otherData.Count)
                {
                    return false;
                }

                for (int i = 0; i < count; i++)
                {
                    var value = fValues[i];
                    if (!value.Equals(otherData[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Timelist({Count()})";
        }
    }
}
