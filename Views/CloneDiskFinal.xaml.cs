using msptool.Button_Commands;
using msptool.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Timer = System.Windows.Forms.Timer;

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
        public CloneDiskFinal()
        {
            InitializeComponent();
            Createtextbox();
        }
        public void Createtextbox()
        {
            if (stackPanel == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: stackPanel XAML element is null.");
            }
            string sourceDisk = null;
            string destinationDisk = null;
            var mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow != null && mainWindow.DataContext is ShellViewModel shellVM)
            {
                sourceDisk = shellVM.SelectedSourceDisk;
                destinationDisk = shellVM.SelectedDestinationDisk;
            }
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(new Run("Source Disk: ") { FontWeight = FontWeights.Bold, FontSize = 16, Foreground = System.Windows.Media.Brushes.White });
            textBlock.Inlines.Add(new Run(sourceDisk) { FontWeight = FontWeights.Regular, FontSize = 16, Foreground = System.Windows.Media.Brushes.White });
            textBlock.Inlines.Add(new Run(Environment.NewLine));
            textBlock.Inlines.Add(new Run("Destination Disk: ") { FontWeight = FontWeights.Bold, FontSize = 16, Foreground = System.Windows.Media.Brushes.White });
            textBlock.Inlines.Add(new Run(destinationDisk) { FontWeight = FontWeights.Regular, FontSize = 16, Foreground = System.Windows.Media.Brushes.White });
            stackPanel.Children.Add(textBlock);
        }

        private void progressBar_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeAndRunTask();

        }
        private static System.Timers.Timer ptimer;
        private void InitializeAndRunTask()
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = 255;
            progressBar.Value = 0;
            OnTimedEvent(null, null);
            ptimer = new System.Timers.Timer(1000); // Update every 1000ms


        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (progressBar.Value < progressBar.Maximum)
                {
                    progressBar.Value += 1;
                }
                else
                {
                    ptimer.Stop();
                    ptimer.Dispose();
                    System.Windows.MessageBox.Show("Disk cloning completed successfully!", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            });
        }
    }
}
