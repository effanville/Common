using System;

namespace Effanville.Common.UI.Events
{
    /// <summary>
    /// Event args with an extra parameter specifying a value.
    /// </summary>
    public sealed class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public EventArgs(T value)
        {
            Value = value;
        }

        /// <summary>
        /// The value to also store.
        /// </summary>
        public T Value
        {
            get; private set;
        }
    }
}
