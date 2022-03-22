using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// Contains naming information, allowing for a primary and secondary name.
    /// If the properties desire to be serialised, one must create wrapping properties for these.
    /// </summary>
    public class Name : IValidity, IComparable, IComparable<Name>, IEquatable<Name>
    {
        /// <summary>
        /// The primary name (the company name)
        /// </summary>
        [XmlIgnore]
        public string PrimaryName
        {
            get;
            set;
        }

        /// <summary>
        /// The secondary name.
        /// </summary>
        [XmlIgnore]
        public string SecondaryName
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor with both names.
        /// </summary>
        public Name(string primaryName, string secondaryName)
        {
            PrimaryName = primaryName;
            SecondaryName = secondaryName;
        }

        /// <summary>
        /// Allows for construction with just one name.
        /// </summary>
        public Name(string primaryName)
        {
            PrimaryName = primaryName;
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public Name()
        {
        }

        /// <summary>
        /// Provides a copy of the given Name.
        /// </summary>
        public Name Copy()
        {
            return new Name(PrimaryName, SecondaryName);
        }

        /// <summary>
        /// Returns whether both the name values are null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(Name name)
        {
            if (name != null)
            {
                if (!string.IsNullOrEmpty(name.PrimaryName) || !string.IsNullOrEmpty(name.SecondaryName))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            //both name and company cannot be null so this is all cases.
            if (string.IsNullOrEmpty(PrimaryName) && !string.IsNullOrEmpty(SecondaryName))
            {
                return $"-{SecondaryName}";
            }
            if (string.IsNullOrEmpty(SecondaryName) && !string.IsNullOrEmpty(PrimaryName))
            {
                return PrimaryName;
            }

            return $"{PrimaryName}-{SecondaryName}";
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is Name otherName)
            {
                return CompareTo(otherName);
            }

            return 0;
        }

        /// <inheritdoc/>
        public int CompareTo(Name other)
        {
            if (PrimaryName == other.PrimaryName)
            {
                return SecondaryName?.CompareTo(other.SecondaryName) ?? (other.SecondaryName == null ? 0 : 1);
            }

            if (PrimaryName == null && other.PrimaryName != null)
            {
                return -1;
            }

            return PrimaryName.CompareTo(other.PrimaryName);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + PrimaryName?.GetHashCode() ?? 0;
            hashCode = 23 * hashCode + SecondaryName?.GetHashCode() ?? 0;
            return hashCode;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Name otherName)
            {
                return Equals(otherName);
            }

            return false;
        }

        /// <summary>
        /// Equal if both names are the same.
        /// Can be used in inherited classes to query the uniqueness of the names.
        /// </summary>
        public bool IsEqualTo(object obj)
        {
            if (obj is Name otherName)
            {
                return Equals(otherName);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Name other)
        {
            bool primaryEqual = PrimaryName?.Equals(other.PrimaryName) ?? string.IsNullOrEmpty(other.PrimaryName);
            bool secondaryEqual = SecondaryName?.Equals(other.SecondaryName) ?? string.IsNullOrEmpty(other.SecondaryName);
            return primaryEqual && secondaryEqual;
        }

        /// <inheritdoc/>
        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        /// <inheritdoc/>
        public List<ValidationResult> Validation()
        {
            List<ValidationResult> output = new List<ValidationResult>();
            output.AddIfNotNull(Validating.IsNotNullOrEmpty(PrimaryName, nameof(PrimaryName), ToString()));
            output.AddIfNotNull(Validating.IsNotNullOrEmpty(SecondaryName, nameof(SecondaryName), ToString()));
            return output;
        }

        /// <summary>
        /// Provides a mechanism to edit the names in this Name.
        /// </summary>
        public void EditName(string primaryName, string secondaryName)
        {
            PrimaryName = primaryName;
            SecondaryName = secondaryName;
        }
    }
}
