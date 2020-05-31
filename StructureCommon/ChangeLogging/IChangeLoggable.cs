using System;

namespace StructureCommon.ChangeLogging
{
    /// <summary>
    /// Interface to enable the logging of what has changed.
    /// </summary>
    public interface IChangeLoggable
    {
        /// <summary>
        /// Event that notifies when data is edited.
        /// The assumption is that this event fires whenever
        /// an object in the implementing class is changed.
        /// </summary>
        event EventHandler DataEdited;
    }
}
