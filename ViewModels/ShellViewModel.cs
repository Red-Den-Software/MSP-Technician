using Caliburn.Micro;
using msptool.Commands;
using msptool.MVVM;
using msptool.ViewModels;
using msptool.Views;
using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace msptool.ViewModels
{
    public class diskInfo
    {
        public string Name { get; set; }
        public string DriveType { get; set; }
        public string VolumeLabel { get; set; }
        public string DriveFormat { get; set; }
        public long TotalSize { get; set; }
        public long AvailableFreeSpace { get; set; }
    }

    public class ShellViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
       => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        public LinearGradientBrush GlassBackgroundBrush { get; } = new LinearGradientBrush
        {
            StartPoint = new System.Windows.Point(0, 0),
            EndPoint = new System.Windows.Point(1, 1),

            GradientStops = new GradientStopCollection
    {
        // Top highlight
        new GradientStop(
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF2A2D32"), 0.0),

        new GradientStop(
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF1F2125"), 0.5),

        new GradientStop(
            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF16181C"), 1.0)
    }
        };
        private CloneDiskViewModel _cloneDiskViewModel;
        private CloneDiskViewModelPg2 _cloneDiskViewModelPg2;

        public ICommand UpdateViewCommand { get; }

        public ShellViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(UpdateView);

            CurrentView = new HomeViewModel();



        }

        public void UpdateView(object parameter)
        {
            switch (parameter?.ToString())
            {
                case "DataView":
                    CurrentView = new DataViewPage1();
                    break;
                case "Tools":
                    CurrentView = new ToolViewModel();
                    break;
                case "Home":
                    CurrentView = new HomeViewModel();
                    break;
                case "CloneDisk":

                    if (_cloneDiskViewModel == null)
                    {
                        _cloneDiskViewModel = new CloneDiskViewModel(this);
                    }

                    CurrentView = _cloneDiskViewModel;
                    break;

                case "CloneDiskPg2":
                    // FIX: You must retrieve 'sourceDisk' from the existing wizard state
                    CurrentView = new CloneDiskViewPg2();
                    break;
                case "CloneDiskFinal":
                    CurrentView = new CloneDiskFinal();
                    break;

            }
        }



    }
    public class DiskSelection : INotifyPropertyChanged
    {
        private string _rootSourceDisk;
        private string _rootDestinationDisk;
        private static readonly DiskSelection _instance = new();

        public static DiskSelection Instance => _instance;

        public string SelectedSourceDisk { get; set; }
        public string SelectedDestinationDisk { get; set; }
        public string RootSourceDisk =>
    GetPhysicalDriveFromLetter(SelectedSourceDisk);

        public string RootDestinationDisk =>
    GetPhysicalDriveFromLetter(SelectedDestinationDisk);


        public event PropertyChangedEventHandler PropertyChanged;

        public static string GetPhysicalDriveFromLetter(string driveLetter)
        {
            driveLetter = driveLetter.TrimEnd('\\');

            using var logicalDisk =
                new ManagementObject($"Win32_LogicalDisk.DeviceID='{driveLetter}'");

            logicalDisk.Get();

            foreach (ManagementObject partition in logicalDisk.GetRelated("Win32_DiskPartition"))
            {
                foreach (ManagementObject disk in partition.GetRelated("Win32_DiskDrive"))
                {
                    return disk["DeviceID"]?.ToString();
                }
            }

            return null;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}





