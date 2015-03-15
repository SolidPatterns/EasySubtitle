using System;
using System.Windows.Input;

namespace EasySubtitle.WPF.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
                _action.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}