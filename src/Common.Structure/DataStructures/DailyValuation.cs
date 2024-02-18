using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Common.Structure.Extensions;

namespace Common.Structure.DataStructures
{
    /// <summary>
    /// Holds a date and a decimal value to act as the value on that day. This is particularly for use
    /// with financial data.
    /// </summary>
    public class DailyValuation :
        IComparable,
        IComparable<DailyValuation>,
        IEquatable<DailyValuation>,
        IXmlSerializable
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
        public decimal Value
        {
            get;
            set;
        }

        /// <summary>
        /// empty constructor.
        /// </summary>
        public DailyValuation()
        {
        }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public DailyValuation(DateTime idealDate, decimal idealValue)
        {
            Day = idealDate;
            Value = idealValue;
        }

        /// <summary>
        /// Creates a new <see cref="DailyValuation"/> from the given one.
        /// </summary>
        public DailyValuation(DailyValuation dailyValue)
            : this(dailyValue.Day, dailyValue.Value)
        {
        }

        /// <summary>
        /// Appends date in UK format with value, separated by a comma.
        /// </summary>
        public override string ToString() => Day.ToUkDateStringPadded() + ", " + Value.ToString();

        /// <inheritdoc/>
        public int CompareTo(DailyValuation other) => DateTime.Compare(Day, other.Day);

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            if (obj is DailyValuation val)
            {
                return CompareTo(val);
            }

            return 0;
        }

        /// <summary>
        /// Returns a copy of the specified valuation
        /// </summary>
        /// <returns></returns>
        public DailyValuation Copy() => new DailyValuation(Day, Value);

        /// <summary>
        /// Sets the data in the daily valuation.
        /// </summary>
        public void SetData(DateTime date, decimal value)
        {
            Day = date;
            Value = value;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is DailyValuation other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(DailyValuation other)
            => Day.Equals(other.Day)
                && Value.Equals(other.Value);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Day.GetHashCode();
            hashCode = 23 * hashCode + Value.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema() => null;

        private const string XmlBaseElement = "DailyValuation";

        private const string XmlDayElement = "Day";
        private const string XmlValueElement = "Value";

        /// <summary>
        /// The old method for reading xml. Used for
        /// backwards compatibility.
        /// </summary>
        public void ReadXmlOld(XmlReader reader)
        {
            reader.ReadStartElement(XmlBaseElement);
            reader.ReadStartElement(XmlDayElement);
            string day = reader.ReadContentAsString();
            reader.ReadEndElement();
            reader.ReadStartElement(XmlValueElement);
            string values = reader.ReadContentAsString();
            reader.ReadEndElement();

            _ = DateTime.TryParse(day, out DateTime date);
            _ = decimal.TryParse(values, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value);

            Day = date;
            Value = value;
            reader.ReadEndElement();
        }

        /// <summary>
        /// The old method for writing xml. No longer used.
        /// </summary>
        public void WriteXmlOld(XmlWriter writer)
        {
            writer.WriteStartElement(XmlBaseElement);
            writer.WriteStartElement(XmlDayElement);
            writer.WriteValue(Day);
            writer.WriteEndElement();
            writer.WriteStartElement(XmlValueElement);
            writer.WriteString(Value.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        private const string XmlBaseElementNew = "DV";

        private const string XmlDayElementNew = "D";
        private const string XmlValueElementNew = "V";

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(XmlBaseElementNew);
            writer.WriteAttributeString(XmlDayElementNew, Day.ToString("yyyy-MM-ddTHH:mm:ss"));
            writer.WriteAttributeString(XmlValueElementNew, Value.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }
        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            // new shorter xml format
            _ = reader.MoveToContent();

            if (reader.Name == XmlBaseElementNew)
            {
                string dayString = reader.GetAttribute(XmlDayElementNew);
                string valueString = reader.GetAttribute(XmlValueElementNew);

                _ = DateTime.TryParse(dayString, out DateTime date);
                _ = decimal.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value);

                Day = date;
                Value = value;
                _ = reader.MoveToElement();
                reader.ReadStartElement();
            }
            else if (reader.Name == XmlBaseElement)
            {
                ReadXmlOld(reader);
            }
            else
            {

            }
        }
    }
}
