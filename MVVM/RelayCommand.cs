using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace msptool.MVVM
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action<object> updateViewCommandExecute)
        {
            UpdateViewCommandExecute = updateViewCommandExecute;
        }

        public RelayCommand(Action<object> updateView, Func<object, bool> value)
        {
            UpdateView = updateView;
        }

        public Action<object> UpdateView { get; }
        public Action<object> UpdateViewCommandExecute { get; }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
