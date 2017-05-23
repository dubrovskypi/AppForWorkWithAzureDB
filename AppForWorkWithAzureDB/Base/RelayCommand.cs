using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppForWorkWithAzureDB.Base
{
    public class RelayCommand : ICommand
    {
        private Action<object> _methodToExecute;
        private Func<object, bool> _canExecuteEvaluator;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> methodToExecute, Func<object, bool> canExecuteEvaluator)
        {

            _methodToExecute = methodToExecute;
            _canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action<object> methodToExecute) : this(methodToExecute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteEvaluator == null) return true;
            else
            {
                bool result = _canExecuteEvaluator.Invoke(parameter);
                return result;
            }
        }

        public void Execute(object parameter)
        {
            _methodToExecute.Invoke(parameter);
        }
    }
}
