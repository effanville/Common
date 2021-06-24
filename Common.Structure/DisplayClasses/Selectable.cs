using System;

namespace Common.Structure.DisplayClasses
{
    /// <summary>
    /// Class that wraps a boolean around an instance of a type <typeparamref name="T"/>. when the boolean 
    /// is changed it raises the <see cref="SelectedChanged"/> event.
    /// </summary>
    /// <typeparam name="T">An object.</typeparam>
    public class Selectable<T>
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
    }
}
