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
    class CloneDiskViewModel : INotifyPropertyChanged
    {
        public ICommand UpdateViewCommand { get; }
        private object _currentView;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public object dataCurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(dataCurrentView));
            }
        }
        private readonly ShellViewModel _parent;

        public CloneDiskViewModel(ShellViewModel parent)
        {

            _parent = parent;

            UpdateViewCommand = new UpdateViewCommand(_parent.UpdateView);
            
        }
    }
}
