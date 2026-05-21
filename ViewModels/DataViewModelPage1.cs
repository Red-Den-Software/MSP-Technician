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

        private readonly DataViewModel _parent;
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
        
        public ICommand UpdateViewCommand { get; }
        public DataViewModelPage1()
        {
           
            UpdateViewCommand = new UpdateViewCommand(UpdateView);
           

        }
        private void UpdateView(object parameter)
        {
            switch (parameter?.ToString())
            {
                case "CloneDisk":
                    dataCurrentView = new CloneDiskView();
                    
                    break;
            }
        }






    }
}
   
