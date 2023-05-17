using System;
using System.Windows.Input;

namespace Common.UI.Commands
{
    /// <summary>
    /// Command instance that executes without an argument required.
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
        private event EventHandler _canExecuteChangedInternal;

        /// <summary>
        /// Constructor that takes an execution method, and can always execute
        /// </summary>
        public RelayCommand(Action execute)
           : this(execute, () => true)
        {
        }

        /// <summary>
        /// Constructor that takes a execution method and whether one can execute.
        /// </summary>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter = null) => _canExecute == null || _canExecute();

        /// <inheritdoc/>
        public void Execute(object obj = null) => _execute();

        /// <summary>
        /// Ensures WPF commanding infrastructure asks all RelayCommand objects whether their
        /// associated views should be enabled whenever a command is invoked.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => _canExecuteChangedInternal += value;
            remove => _canExecuteChangedInternal -= value;
        }

        /// <summary>
        /// Invokes the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = _canExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
