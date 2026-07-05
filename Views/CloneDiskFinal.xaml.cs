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
using msp.Commands;
using DiskAccessLibrary;

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
        private DiskReader diskReader;
        private Disk source_disk;
        private Disk target_disk;
        public CloneDiskFinal()
        {
            InitializeComponent();

            shellVM = System.Windows.Application.Current.MainWindow.DataContext as ShellViewModel;
            
            Loaded += CloneDiskFinal_Loaded;


        }

        private async void CloneDiskFinal_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= CloneDiskFinal_Loaded;

            Createtextbox(
                DiskSelection.Instance.RootSourceDisk,
                DiskSelection.Instance.SelectedDestinationDisk);

            DriveInfo srcDrive = new(DiskSelection.Instance.RootSourceDisk);
            DriveInfo dstDrive = new(DiskSelection.Instance.SelectedDestinationDisk);

            DriveType srcType = srcDrive.DriveType;
            DriveType dstType = dstDrive.DriveType;

            // Raw removable disk clone
            if (srcType == DriveType.Removable &&
                dstType == DriveType.Removable)
            {
                
                DiskCopier diskCopier = new DiskCopier(DiskSelection.Instance.RootSourceDisk, DiskSelection.Instance.RootDestinationDisk);

                return;
            }

            // Validate fixed drives
            if (srcType != DriveType.Fixed)
            {
                MessageBox.Show(
                    $"Source disk must be fixed. Found: {srcType}");
                return;
            }

            if (dstType != DriveType.Fixed)
            {
                MessageBox.Show(
                    $"Destination disk must be fixed. Found: {dstType}");
                return;
            }

            // Normal clone
            CloneDisk cloneDisk = new();

            await Task.Run(() =>
            {
                cloneDisk.StartBackup();
            });
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
       
        private async Task StartCloningAsync()
        {
            System.Diagnostics.Debug.WriteLine("StartCloningAsync method called.");
           
            
            var heavyOperation = new HeavyOperation();
            
           
        }
        public  void UpdateProgress(int current, int total)
        {
            progressBar.Value = (int)((double)current / total * 100);
            progressBar.Maximum = total;
        }
        private async void progressBar_Loaded(object sender, RoutedEventArgs e)
        {
            await StartCloningAsync();
        }

        
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            

        }

    }
    
}
