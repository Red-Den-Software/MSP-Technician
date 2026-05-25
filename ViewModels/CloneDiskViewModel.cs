using msptool.Commands.UpdateViewCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

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
        private System.Windows.Media.Brush _bbrush;
        private Thickness _dstThick;
        private System.Windows.Media.Brush _dstbrush;
        private Thickness _bthi;
        public System.Windows.Media.Brush srcBrush
        {
            get => _bbrush;
            set
            {
                _bbrush = value;
                OnPropertyChanged(nameof(srcBrush));

            }
        }
        public Thickness srcThick
        {
            get => _bthi;
            set
            {
                _bthi = value;
                OnPropertyChanged(nameof(srcThick));

            }
        }
        public string source_disk
        {
            get => _srcdisk;
            set
            {
               
                _srcdisk = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(source_disk)));
               
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
        public Thickness dstThick
        {
            get => _dstThick;
            set
            {
                _dstThick = value;
                OnPropertyChanged(nameof(dstThick));
            }
        }
        public System.Windows.Media.Brush dstBrush
        {
            get => _dstbrush;
            set
            {
                _dstbrush = value;
                OnPropertyChanged(nameof(dstBrush));
            }
        }
        public CloneDiskViewModel(ShellViewModel parent)
        {

            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent.UpdateView);

            source_disk ??= "Source Disk";
            dest_disk ??= "Destination Disk";
            
        }

        public CloneDiskViewModel()
        {
        }

        public string src_disk()
        {
            
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
               
                dialog.SelectedPath = source_disk;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                dialog.Description = "Please select a drive or a root folder.";
                dialog.ShowNewFolderButton = false;

                
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    source_disk = dialog.SelectedPath;
                    srcBrush = Brushes.LimeGreen;
                    srcThick = new Thickness(2);
                    
                    return source_disk;
                }
                return null;
            }
        }
        public string dst_disk()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
               
                dialog.SelectedPath = dest_disk;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                dialog.Description = "Please select a drive or a root folder.";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    
                    dest_disk = dialog.SelectedPath;
                    return dest_disk;
                }
                return null;
            }
        }
        public void checkIfselection()
        {
            if (source_disk==string.Empty || dest_disk == string.Empty)
            {
                if (string.IsNullOrEmpty(dest_disk))
                {
                    dstBrush = Brushes.Red;
                    dstThick = new Thickness(2);
                }
                if (string.IsNullOrEmpty(source_disk))
                {
                    srcBrush = Brushes.Red;
                    srcThick = new Thickness(2);
                }
            }
        }
        private void ShowPopup(PopupViewModel vm)
        {
            var popup = new PopupWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                DataContext = vm
            };

            popup.Show();
        }
        public void showCloneWarning()
        {
            var vm = new PopupViewModel
            {

                EllipseVisibility = Visibility.Hidden,
                mVisibility = Visibility.Hidden,
                nVisibility = Visibility.Visible,
                YVisibility = Visibility.Visible,
                rbutText = "Yes",
                lbutText = "No",
                mbuttonthickness = "0",
                rbuttonthickness = "2",
                lbuttonthickness = "2",


            };

            ShowPopup(vm);
        }
        

    }
}
