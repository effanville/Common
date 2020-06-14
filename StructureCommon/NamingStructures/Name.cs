using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace StructureCommon.NamingStructures
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

        /// <summary>
        /// Returns whether another object is the same as this one.
        /// </summary>
        public override bool Equals(object obj)
        {
            return EqualityMethod(obj);
        }


        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return PrimaryName == null ? 0 : PrimaryName.GetHashCode() + 10 ^ 12 * (SecondaryName == null ? 0 : SecondaryName.GetHashCode());
        }

        /// <summary>
        /// Equal if both names are the same.
        /// Can be used in inherited classes to query the uniqueness of the names.
        /// </summary>
        public bool IsEqualTo(object obj)
        {
            return EqualityMethod(obj);
        }

        /// <summary>
        /// The method by which one determines equality.
        /// </summary>
        private bool EqualityMethod(object obj)
        {
            if (obj is Name otherName)
            {
                if (otherName == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(PrimaryName) && string.IsNullOrEmpty(SecondaryName))
                {
                    if (string.IsNullOrEmpty(otherName.PrimaryName) && string.IsNullOrEmpty(otherName.SecondaryName))
                    {
                        return true;
                    }

                    return false;
                }
                if (string.IsNullOrEmpty(PrimaryName))
                {
                    if (string.IsNullOrEmpty(otherName.PrimaryName))
                    {
                        return SecondaryName.Equals(otherName.SecondaryName);
                    }

                    return false;
                }

                if (string.IsNullOrEmpty(SecondaryName))
                {
                    if (string.IsNullOrEmpty(otherName.SecondaryName))
                    {
                        return PrimaryName.Equals(otherName.PrimaryName);
                    }

                    return false;
                }

                if (PrimaryName.Equals(otherName.PrimaryName) && SecondaryName.Equals(otherName.SecondaryName))
                {
                    return true;
                }
            }

            return false;
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
        public bool EditName(string primaryName, string secondaryName)
        {
            PrimaryName = primaryName;
            SecondaryName = secondaryName;
            return true;
        }
    }
}
