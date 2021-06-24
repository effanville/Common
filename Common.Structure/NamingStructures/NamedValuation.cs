using System;
using Common.Structure.DataStructures;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// Wraps a <see cref="Name"/> around a <see cref="DailyValuation"/>.
    /// </summary>
    public class NamedValuation : DailyValuation
    {
        /// <summary>
        /// Names associated to the values.
        /// </summary>
        public Name Names
        {
            get;
            set;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public NamedValuation()
            : base()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NamedValuation(string company, string name, DateTime day, double value)
            : base(day, value)
        {
            Names = new Name(company, name);
        }

        /// <summary>
        /// Constructor to create an instance from a base class instance.
        /// </summary>
        public NamedValuation(string company, string name, DailyValuation toAddOnto)
            : this(company, name, toAddOnto.Day, toAddOnto.Value)
        {
        }        /// <inheritdoc/>
        public override int CompareTo(object obj)
        {
            if (obj is NamedValuation value)
            {
                return Names.CompareTo(value.Names) + base.CompareTo(value);
            }

            return 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Names.ToString() + "-" + base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is NamedValuation other)
            {
                return Names.Equals(other.Names) && base.Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Names.GetHashCode();
            hashCode = 23 * hashCode + base.GetHashCode();
            return hashCode;
        }
    }
}
