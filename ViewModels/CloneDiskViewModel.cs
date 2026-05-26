using Caliburn.Micro;
using msptool.Commands.UpdateViewCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace msptool.ViewModels
{
    class CloneDiskViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private readonly ShellViewModel _parent;
        public ICommand UpdateViewCommand { get; }
        public CloneDiskViewModel(ShellViewModel parent)
        {
            _parent = parent;

            UpdateViewCommand = new UpdateViewCommand(_parent.UpdateView);
        }
    }
}
