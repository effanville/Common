using System;

namespace Common.UI
{
    /// <summary>
    /// A thin interface around the Threading.Dispatcher class
    /// to abstract dependencies.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Invokes the provided action.
        /// </summary>
        void Invoke(Action action);

        /// <summary>
        ///Executes the specified delegate asynchronously.
        /// </summary>
        void BeginInvoke(Action action);
    }
}
