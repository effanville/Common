using System;

namespace Effanville.Common.Structure.NamingStructures
{
    /// <summary>
    /// A wrapper for adding an object of type <typeparamref name="S"/> as a label to an object of type
    /// <typeparamref name="T"/> where <typeparamref name="T"/> implements
    /// the <see cref="IComparable"/> interface.
    /// </summary>
    public sealed class Labell<S, T> : IEquatable<Labell<S, T>>, IComparable
        where S : IComparable<S>, IEquatable<S>
    {
        /// <summary>
        /// The name to use.
        /// </summary>
        public S Label
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
        public Labell()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Labell(S label, T instance)
        {
            Label = label;
            Instance = instance;
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is Labell<S, T> value)
            {
                return Label.CompareTo(value.Label);
            }

            return 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Label.ToString() + "-" + Instance.ToString();
        }

        /// <inheritdoc/>
        public bool Equals(Labell<S, T> other)
        {
            return Label.Equals(other.Label) && Instance.Equals(other.Instance);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Labell<S, T> other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Label.GetHashCode();
            hashCode = 23 * hashCode + Instance.GetHashCode();
            return hashCode;
        }
    }
}
