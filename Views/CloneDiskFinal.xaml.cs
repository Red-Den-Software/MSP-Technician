using Alphaleonis.Win32.Filesystem;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using msptool.Button_Commands;
using msptool.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using VSS;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DriveInfo = System.IO.DriveInfo;
using MessageBox = System.Windows.MessageBox;
using DiskAccessLibrary;
using msptool.Functions;

namespace msptool.Views
{
    /// <summary>
    /// Interaction logic for CloneDiskFinal.xaml
    /// </summary>
    public partial class CloneDiskFinal : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private CloneDisk cloneDisk;
        private ShellViewModel shellVM;
        private Disk source_disk;
        private Disk target_disk;
        public CloneDiskFinal()
        {
            InitializeComponent();

            shellVM = System.Windows.Application.Current.MainWindow.DataContext as ShellViewModel;
            
            Loaded += CloneDiskFinal_Loaded;


        }
        public bool InitializeClone()
        {
            Createtextbox(
                DiskSelection.Instance.SelectedSourceDisk,
                DiskSelection.Instance.SelectedDestinationDisk);

            DriveInfo srcDrive = new(DiskSelection.Instance.SelectedSourceDisk);
            DriveInfo dstDrive = new(DiskSelection.Instance.SelectedDestinationDisk);

            if (srcDrive.DriveType == DriveType.Removable &&
                dstDrive.DriveType == DriveType.Removable)
            {
               
                return true;    // Raw clone
            }

            return false;       // Use normal clone
        }
        private async void CloneDiskFinal_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= CloneDiskFinal_Loaded;

            InitializeClone();
        }

        public void Createtextbox(string sourceDisk, string destinationDisk)
        {
            if (stackPanel == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: stackPanel XAML element is null.");
            }
            var mainWindow = System.Windows.Application.Current.MainWindow;
           
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run("Source Disk: ") { FontWeight = FontWeights.Bold, FontSize = 16, Foreground = System.Windows.Media.Brushes.White });
            textBlock.Inlines.Add(new Run(sourceDisk) { FontWeight = FontWeights.Regular, FontSize = 16, Foreground = System.Windows.Media.Brushes.White });
            textBlock.Inlines.Add(new Run(Environment.NewLine));
            textBlock.Inlines.Add(new Run("Destination Disk: ") { FontWeight = FontWeights.Bold, FontSize = 16, Foreground = System.Windows.Media.Brushes.White });
            textBlock.Inlines.Add(new Run(destinationDisk) { FontWeight = FontWeights.Regular, FontSize = 16, Foreground = System.Windows.Media.Brushes.White });
            stackPanel.Children.Add(textBlock);
        }

    }
    
}
