using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Single.Core
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;
        public DelegateCommand(Action execute)
        {
            this._execute = execute;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
