using System;

namespace StructureCommon.DisplayClasses
{
    /// <summary>
    /// Class that wraps a boolean around an instance of a type <typeparamref name="T"/>. when the boolean 
    /// is changed it raises the <see cref="SelectedChanged"/> event.
    /// </summary>
    /// <typeparam name="T">An object that implements the <see cref="IEquatable{T}"/> interface.</typeparam>
    public class Selectable<T> where T : IEquatable<T>
    {
        private bool fSelected;
        /// <summary>
        /// A boolean flag associated to the <see cref="Instance"/>.
        /// </summary>
        public bool Selected
        {
            get
            {
                return fSelected;
            }
            set
            {
                if (fSelected != value)
                {
                    fSelected = value;
                    OnSelectedChanged();
                }
            }
        }

        /// <summary>
        /// The instance to hold.
        /// </summary>
        public T Instance
        {
            get;
            set;
        }

        public Selectable(T instance, bool selected)
        {
            Selected = selected;
            Instance = instance;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Selectable()
        {
            Selected = false;
            Instance = default(T);
        }

        /// <summary>
        /// Event handler controlling property changed
        /// </summary>
        public event EventHandler SelectedChanged;

        /// <summary>
        /// Invokes the event for The Selected nature of the item has changed.
        /// </summary>
        private void OnSelectedChanged()
        {
            if (SelectedChanged != null)
            {
                SelectedChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Determines equality of this object. This is solely equality <br/>
        /// based upon the <see cref="Instance"/> and is not determined<br/>
        /// by the <see cref="Selected"/> propety.
        /// </summary>
        public bool Equals(T other)
        {
            return Instance?.Equals(other) ?? other != null;
        }

        /// <summary>
        /// General mechanism for determining equality.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Selectable<T> selec)
            {
                return Equals(selec);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode + 23 * Instance.GetHashCode();
            return hashCode;
        }
    }
}
