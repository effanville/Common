using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Effanville.Common.Structure.Extensions;

namespace Effanville.Common.Structure.DataStructures.Numeric
{
    /// <summary>
    /// Holds a date and a double value to act as the value on that day.
    /// </summary>
    public class DailyNumeric : IComparable, IComparable<DailyNumeric>, IEquatable<DailyNumeric>, IXmlSerializable
    {
        /// <summary>
        /// The date for the valuation
        /// </summary>
        public DateTime Day
        {
            get;
            set;
        }

        /// <summary>
        /// The specific valuation
        /// </summary>
        public double Value
        {
            get;
            set;
        }

        /// <summary>
        /// empty constructor.
        /// </summary>
        public DailyNumeric()
        {
        }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public DailyNumeric(DateTime idealDate, double idealValue)
        {
            Day = idealDate;
            Value = idealValue;
        }

        /// <summary>
        /// Creates a new <see cref="DailyNumeric"/> from the given one.
        /// </summary>
        public DailyNumeric(DailyNumeric dailyValue)
            : this(dailyValue.Day, dailyValue.Value)
        {
        }

        /// <summary>
        /// Appends date in UK format with value, separated by a comma.
        /// </summary>
        public override string ToString()
        {
            return Day.ToUkDateStringPadded() + ", " + Value.ToString();
        }

        /// <inheritdoc/>
        public int CompareTo(DailyNumeric other)
        {
            return DateTime.Compare(Day, other.Day);
        }

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            if (obj is DailyNumeric val)
            {
                return CompareTo(val);
            }

            return 0;
        }

        /// <summary>
        /// Returns a copy of the specified valuation
        /// </summary>
        /// <returns></returns>
        public DailyNumeric Copy()
        {
            return new DailyNumeric(Day, Value);
        }

        /// <summary>
        /// Sets the data in the daily valuation.
        /// </summary>
        public void SetData(DateTime date, double value)
        {
            Day = date;
            Value = value;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is DailyNumeric other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(DailyNumeric other)
        {
            return Day.Equals(other.Day) && Value.Equals(other.Value);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Day.GetHashCode();
            hashCode = 23 * hashCode + Value.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        private const string XmlBaseElement = "DV";

        private const string XmlDayElement = "D";
        private const string XmlValueElement = "V";

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(XmlBaseElement);
            writer.WriteAttributeString(XmlDayElement, Day.ToString("yyyy-MM-ddTHH:mm:ss"));
            writer.WriteAttributeString(XmlValueElement, Value.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }
        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            // new shorter xml format
            _ = reader.MoveToContent();

            if (reader.Name == XmlBaseElement)
            {
                string dayString = reader.GetAttribute(XmlDayElement);
                string valueString = reader.GetAttribute(XmlValueElement);

                _ = DateTime.TryParse(dayString, out DateTime date);
                _ = double.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);

                Day = date;
                Value = value;
                _ = reader.MoveToElement();
                reader.ReadStartElement();
            }
            else
            {
                throw new XmlException("Xml for DailyNumeric not of the correct form.");
            }
        }
    }
}
