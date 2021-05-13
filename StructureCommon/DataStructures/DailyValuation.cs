using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using StructureCommon.Extensions;

namespace StructureCommon.DataStructures
{
    /// <summary>
    /// Holds a date and a value to act as the value on that day.
    /// </summary>
    public class DailyValuation : IComparable, IXmlSerializable
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
        public DailyValuation()
        {
        }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public DailyValuation(DateTime idealDate, double idealValue)
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
        public override string ToString()
        {
            return Day.ToUkDateStringPadded() + ", " + Value.ToString();
        }

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            DailyValuation a = (DailyValuation)obj;
            return DateTime.Compare(Day, a.Day);
        }

        /// <summary>
        /// Returns a copy of the specified valuation
        /// </summary>
        /// <returns></returns>
        public DailyValuation Copy()
        {
            return new DailyValuation(Day, Value);
        }

        /// <summary>
        /// Sets the data in the daily valuation.
        /// </summary>
        public void SetData(DateTime date, double value)
        {
            Day = date;
            Value = value;
        }

        /// <summary>
        /// Sets the day field only.
        /// </summary>
        public void SetDay(DateTime date)
        {
            Day = date;
        }

        /// <summary>
        /// Sets the value field.
        /// </summary>
        public void SetValue(double value)
        {
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

        public bool Equals(DailyValuation other)
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        private const string XmlBaseElement = "DailyValuation";

        private const string XmlDayElement = "Day";
        private const string XmlValueElement = "Value";

        /// <inheritdoc/>
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
            _ = double.TryParse(values, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);

            Day = date;
            Value = value;
        }

        /// <inheritdoc/>
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
            bool thing = reader.Name == XmlBaseElementNew;

            if (thing)
            {
                string dayString = reader.GetAttribute(XmlDayElementNew);
                string valueString = reader.GetAttribute(XmlValueElementNew);

                _ = DateTime.TryParse(dayString, out DateTime date);
                _ = double.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);

                Day = date;
                Value = value;
            }
            else
            {
                ReadXmlOld(reader);
            }
        }

    }
}
