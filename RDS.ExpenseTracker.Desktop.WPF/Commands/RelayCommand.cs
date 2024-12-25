using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RDS.ExpenseTracker.Desktop.WPF.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action<object> executeMethod;
        private Predicate<object> canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> predicate)
        {
            executeMethod = execute;
            canExecute = predicate;
        }

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
            
        }

        public bool CanExecute(object? parameter)
        {
            return parameter == null ? true : canExecute.Invoke(parameter);
        }

        public void Execute(object? parameter)
        {
            executeMethod.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
