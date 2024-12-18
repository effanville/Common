﻿using System;

using Effanville.Common.Structure.DataStructures;

namespace Effanville.Common.Structure.NamingStructures
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
        public NamedValuation(string primaryName, string secondaryName, DateTime day, decimal value)
            : base(day, value)
        {
            Names = new Name(primaryName, secondaryName);
        }

        /// <summary>
        /// Constructor to create an instance from a base class instance.
        /// </summary>
        public NamedValuation(string primaryName, string secondaryName, DailyValuation toAddOnto)
            : this(primaryName, secondaryName, toAddOnto.Day, toAddOnto.Value)
        {
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
