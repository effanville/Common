using System;

namespace Common.UI.Events
{
    /// <summary>
    /// Contains extension methods for an event handler.
    /// </summary>
    public static class EventRaiser
    {
        /// <summary>
        /// Raises an event with the object as the invoking object.
        /// </summary>
        /// <param name="handler">The handler to raise with.</param>
        /// <param name="sender">The sender object.</param>
        public static void Raise(this EventHandler handler, object sender)
        {
            handler?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Raises an event with the object as the invoking object.
        /// </summary>
        /// <param name="handler">The handler to raise with.</param>
        /// <param name="sender">The sender object.</param>
        /// <param name="value">The value for the event args.</param>
        public static void Raise<T>(this EventHandler<EventArgs<T>> handler, object sender, T value)
        {
            handler?.Invoke(sender, new EventArgs<T>(value));
        }

        /// <summary>
        /// Raises an event with the object as the invoking object.
        /// </summary>
        /// <param name="handler">The handler to raise with.</param>
        /// <param name="sender">The sender object.</param>
        /// <param name="value">The value for the event args.</param>
        public static void Raise<T>(this EventHandler<T> handler, object sender, T value)
            where T : EventArgs
        {
            handler?.Invoke(sender, value);
        }

        /// <summary>
        /// Raises an event with the object as the invoking object.
        /// </summary>
        /// <param name="handler">The handler to raise with.</param>
        /// <param name="sender">The sender object.</param>
        /// <param name="value">The value for the event args.</param>
        public static void Raise<T>(this EventHandler<EventArgs<T>> handler, object sender, EventArgs<T> value)
        {
            handler?.Invoke(sender, value);
        }
    }
}
