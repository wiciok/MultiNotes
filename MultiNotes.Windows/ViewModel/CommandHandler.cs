using System;
using System.Windows.Input;

namespace MultiNotes.Windows.ViewModel
{
    public class CommandHandler : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public CommandHandler(Action<object> execute, Predicate<object> canExecute=null)
        {
            if (execute != null)
                _execute = execute;
            else
                throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
