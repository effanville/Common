using System;
using System.Windows.Input;

namespace Common.UI.Commands
{
    /// <summary>
    /// Command instance that executes with an argument that is an object.
    /// </summary>
    public sealed class RelayCommandObject : ICommand
    {
        private readonly Action<object> fExecute;

        private readonly Predicate<object> fCanExecute;

        private event EventHandler fCanExecuteChangedInternal;

        /// <summary>
        /// Constructor that takes an execution method, and can always execute
        /// </summary>
        public RelayCommandObject(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Constructor that takes a execution method and whether one can execute.
        /// </summary>
        public RelayCommandObject(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            fExecute = execute;
            fCanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                fCanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                fCanExecuteChangedInternal -= value;
            }
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return fCanExecute != null && fCanExecute(parameter);
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            fExecute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = fCanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
