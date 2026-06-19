using Alphaleonis.Win32.Filesystem;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using msptool.Button_Commands;
using msptool.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using VSS;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private string source_disk;
        private string target_disk;
        public CloneDiskFinal()
        {
            InitializeComponent();

            shellVM = System.Windows.Application.Current.MainWindow.DataContext as ShellViewModel;
            
            Loaded += CloneDiskFinal_Loaded;


        }

        private async void CloneDiskFinal_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= CloneDiskFinal_Loaded;

            shellVM = System.Windows.Application.Current.MainWindow.DataContext as ShellViewModel;

            Createtextbox(
                DiskSelection.Instance.SelectedSourceDisk,
                DiskSelection.Instance.SelectedDestinationDisk);

            var cloneDisk = new CloneDisk();

            await Task.Run(() => cloneDisk.StartBackup());
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
        public int ProgressValue
        {
            get => (int)progressBar.Value;
            set
            {
                if (progressBar.Value != value)
                {
                    progressBar.Value = value;
                    OnPropertyChanged(nameof(ProgressValue));
                }
            }
        }
       
        private async Task StartCloningAsync()
        {
            System.Diagnostics.Debug.WriteLine("StartCloningAsync method called.");
           
            var progress = new Progress<int>(value => ProgressValue = value);
            var heavyOperation = new HeavyOperation();
            await heavyOperation.RunProcessAsync(progress);
           
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
