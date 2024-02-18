using System;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// A wrapper for adding a <see cref="Name"/> to an object of type
    /// <typeparamref name="T"/> where <typeparamref name="T"/> implements
    /// the <see cref="IComparable"/> interface.
    /// </summary>
    public sealed class Named<T> where T : IComparable
    {
        /// <summary>
        /// The name to use.
        /// </summary>
        public Name Names
        {
            get;
            set;
        }

        /// <summary>
        /// The object to add a name to.
        /// </summary>
        public T Instance
        {
            get;
            set;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Named()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Named(string primaryName, string secondaryName, T instance)
        {
            Names = new Name(primaryName, secondaryName);
            Instance = instance;
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is Named<T> value)
            {
                return Names.CompareTo(value.Names) + Instance.CompareTo(value.Instance);
            }

            return 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Names.ToString() + "-" + Instance.ToString();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Named<T> other)
            {
                return Names.Equals(other.Names) && Instance.Equals(other.Instance);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Names.GetHashCode();
            hashCode = 23 * hashCode + Instance.GetHashCode();
            return hashCode;
        }
    }
}
