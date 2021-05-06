using System;
using System.Windows.Input;

namespace ConnectFour.ViewModel
{
    public class RelayCommand : ICommand
    {
        private Action<object> _action;
        private Predicate<object> _predicate;

        public RelayCommand(Action<object> action, Predicate<object> predicate)
        {
            _action = action;
            _predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            return _predicate(parameter);
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}