using System;
using System.Windows.Input;

namespace Common.UI.Commands
{
    /// <summary>
    /// Command instance that executes with an argument of the specified type T.
    /// </summary>
    public sealed class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        private event EventHandler _canExecuteChangedInternal;

        /// <summary>
        /// Constructor that takes an execution method, and can always execute
        /// </summary>
        public RelayCommand(Action<T> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Constructor that takes a execution method and whether one can execute.
        /// </summary>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Ensures WPF commanding infrastructure asks all RelayCommand objects whether their
        /// associated views should be enabled whenever a command is invoked.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => _canExecuteChangedInternal += value;

            remove => _canExecuteChangedInternal -= value;
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => _canExecute != null && _canExecute((T)parameter);

        /// <inheritdoc/>
        public void Execute(object parameter) => _execute((T)parameter);

        /// <summary>
        /// Invokes the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = _canExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private static bool DefaultCanExecute(T parameter) => true;
    }
}
