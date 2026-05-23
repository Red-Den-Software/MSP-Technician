using msptool.Commands.UpdateViewCommand;
using msptool.MVVM;
using msptool.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace msptool.ViewModels
{
    public class DataViewModelPage1 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        
        private object _currentView;
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
        public ICommand UpdateViewCommand { get; }
       
        public DataViewModelPage1(ShellViewModel parent)
        {
            _parent = parent;

            UpdateViewCommand = new UpdateViewCommand(_parent.UpdateView);
        }








    }
}
   
