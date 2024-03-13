using System;
using System.Windows;
using System.Windows.Threading;

namespace Effanville.Common.UI.Wpf
{
    /// <summary>
    /// An implementation of <see cref="IDispatcher"/> using
    /// the <see cref="Application.Current.Dispatcher"/> to
    /// dispatch.
    /// </summary>
    public class DispatcherInstance : IDispatcher
    {
        private readonly Dispatcher fDispatcher;

        /// <summary>
        /// Create an instance.
        /// </summary>
        public DispatcherInstance()
        {
            fDispatcher = Application.Current.Dispatcher;
        }

        /// <inheritdoc/>
        public void Invoke(Action action)
        {
            fDispatcher.Invoke(action);
        }

        /// <inheritdoc/>
        public void BeginInvoke(Action action)
        {
            _ = fDispatcher.BeginInvoke(action);
        }
    }
}
