using System;

namespace Effanville.Common.Structure.DataEdit
{
    /// <summary>
    /// Contains methods for updating an object of type <see typeparamcref="T"/> using callbacks.
    /// </summary>
    public interface IDataStoreUpdater<T> where T : class
    {
        /// <summary>
        /// The underlying instance to update.
        /// </summary>
        T Database
        {
            get;
            set;
        }

        /// <summary>
        /// Update the portfolio with the given action.
        /// </summary>
        void PerformUpdateAction(Action<T> action, T portfolio);

        /// <summary>
        /// Update the portfolio with the given action.
        /// </summary>
        void PerformUpdate(object obj, UpdateRequestArgs<T> requestArgs);
    }
}
