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
    public class Name : IValidity, IComparable
    {
        /// <summary>
        /// The primary name (the company name)
        /// </summary>
        [XmlIgnoreAttribute]
        public string PrimaryName
        {
            get;
            set;
        }

        /// <summary>
        /// The secondary name.
        /// </summary>
        [XmlIgnoreAttribute]
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

        /// <summary>
        /// Display of names, and allows for null name values.
        /// </summary>
        public override string ToString()
        {
            //both name and company cannot be null so this is all cases.
            if (string.IsNullOrEmpty(PrimaryName) && !string.IsNullOrEmpty(SecondaryName))
            {
                return SecondaryName;
            }
            if (string.IsNullOrEmpty(SecondaryName) && !string.IsNullOrEmpty(PrimaryName))
            {
                return PrimaryName;
            }

            return SecondaryName + " " + PrimaryName;
        }

        /// <summary>
        /// Compares both names.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is Name value)
            {
                if (PrimaryName == value.PrimaryName)
                {
                    if (SecondaryName == null)
                    {
                        if (value.SecondaryName == null)
                        {
                            return 0;
                        }
                        return 1;
                    }
                    return SecondaryName.CompareTo(value.SecondaryName);
                }
                if (PrimaryName == null && value.PrimaryName != null)
                {
                    return -1;
                }
                return PrimaryName.CompareTo(value.PrimaryName);
            }

            return 0;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + PrimaryName.GetHashCode();
            hashCode = 23 * hashCode + SecondaryName.GetHashCode();
            return hashCode;
        }
        /// <summary>
        /// Returns whether another object is the same as this one.
        /// </summary>
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
            bool primaryEqual = PrimaryName?.Equals(other.PrimaryName) ?? other.PrimaryName == null;
            bool secondaryEqual = SecondaryName?.Equals(other.SecondaryName) ?? other.SecondaryName == null;
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
