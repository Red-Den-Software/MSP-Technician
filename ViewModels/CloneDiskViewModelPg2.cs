using Caliburn.Micro;
using msptool.Commands.UpdateViewCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace msptool.ViewModels
{
    class CloneDiskViewModelPg2
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private readonly ShellViewModel _parent;
        public ICommand UpdateViewCommand { get; }
      

        public CloneDiskViewModelPg2(ShellViewModel parent)
        {
            _parent = parent;

            UpdateViewCommand = new UpdateViewCommand(_parent.UpdateView);
            
        }
        

    }
}
