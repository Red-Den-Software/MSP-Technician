using Caliburn.Micro;
using msptool.Commands.UpdateViewCommand;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;

namespace msptool.ViewModels
{
    class CloneDiskViewModelPg2
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private readonly ShellViewModel _parent;
        public ICommand UpdateViewCommand { get; }
      

        public CloneDiskViewModelPg2(ShellViewModel parent, string SourceDisk_ValueVM)
        {
           
            SourceDisk = SourceDisk_ValueVM;

        }
        public CloneDiskViewModelPg2()
        {

        }
        
        private string _sourceDisk;
        public string SourceDisk
        {
            get => _sourceDisk;
            set
            {
                _sourceDisk = value;
                OnPropertyChanged(nameof(SourceDisk));
            }
        }

       

    }
}
