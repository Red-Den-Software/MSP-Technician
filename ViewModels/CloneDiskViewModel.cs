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
        private string _srcdisk;
        private CloneDiskViewModel cloneDiskViewModel;
        private string _destdisk;
        public string source_disk
        {
            get => _srcdisk;
            set
            {
                _srcdisk = value;
                OnPropertyChanged(nameof(source_disk));
            }
        }
        public string dest_disk
        {
            get => _destdisk;
            set
            {
                _destdisk = value;
                OnPropertyChanged(nameof(dest_disk));
            }
        }
        public CloneDiskViewModel(ShellViewModel parent)
        {

            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent.UpdateView);

            if (string.IsNullOrEmpty(_srcdisk) || string.IsNullOrEmpty(_destdisk))
            {
                _srcdisk = "Source Disk";
                _destdisk = "Destination Disk";
            }
            
        }

       

        public void folderDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            dialog.SelectedPath = _srcdisk;
        }
    }
}
