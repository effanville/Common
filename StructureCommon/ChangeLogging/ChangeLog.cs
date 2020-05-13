using System;

namespace StructureCommon.ChangeLogging
{
    /// <summary>
    /// Class that can store information on whether something in the type T has been edited.
    /// </summary>
    /// <typeparam name="T">Object that implements <typeparamref name="IChangeLoggable"/></typeparam>
    public class ChangeLog<T> where T : IChangeLoggable
    {
        /// <summary>
        /// The value to record whether it has changed.
        /// </summary>
        T valueToLogChanges;

        /// <summary>
        /// Whether a value in <see cref="valueToLogChanges"/> is a new value or not.
        /// </summary>
        public bool NewValue
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Constructor taking instance to log.
        /// </summary>
        public ChangeLog(T loggable)
        {
            valueToLogChanges = loggable;
            loggable.DataEdited += SetValueEdited;
        }

        /// <summary>
        /// Function to set the logging.
        /// </summary>
        private void SetValueEdited(object edited, EventArgs e)
        {
            NewValue = true;
        }
    }
}
