using Microsoft.Win32;
using msptool.Button_Commands;
using msptool.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using MessageBox = System.Windows.Forms.MessageBox;
using RadioButton = System.Windows.Controls.RadioButton;
using static msptool.Functions.LowLevelDiskCopy;
namespace msptool.Views
{
    public partial class CloneDiskView : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private readonly ShellViewModel _shellVM;
        public CloneDiskView()
        {
           
            InitializeComponent();
            CreateButtons();
            

        }
        public CloneDiskView(ShellViewModel shellVM) : this() // ': this()' calls the constructor above first
        {
            _shellVM = shellVM;
        }

        private CloneDiskView _parentView;
        private ShellViewModel ShellViewModel;
        
        
        public void CreateButtons()
        {
            List<DriveItem> driveItems = GetDiskInfo();

            foreach (DriveItem driveItem in driveItems)
            {
                System.Windows.Controls.RadioButton driveButton = new System.Windows.Controls.RadioButton();
                driveButton.GroupName = "Drives";
                driveButton.Content = driveItem.DisplayName;
                driveButton.Style = (Style)TryFindResource("cloneDiskBut");
                driveButton.Tag = driveItem.DeviceID;
                driveButton.Click += DriveButton_Click;
                ButtonPanelClone.Children.Add(driveButton);
                 // Set a breakpoint here to inspect the driveButton properties during runtime
            }
        }
        private void DriveButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton button = sender as RadioButton;
            if (button == null)
                return;

            if (button.Tag == null)
            {
                Debugger.Break();
                System.Diagnostics.Debug.WriteLine("Error: Drive button Tag is null or empty.");
                return;
            }
            string rootPath = button.Tag.ToString();
            ShellViewModel shellVM = System.Windows.Application.Current.MainWindow.DataContext as ShellViewModel;
            DiskCheck diskCheck = new DiskCheck();
            diskCheck.CheckDiskType(rootPath);
            DiskSelection.Instance.SelectedSourceDisk = rootPath;
            System.Diagnostics.Debug.WriteLine($"Selected Source Disk: {rootPath}");

        }
        
    }

}
   
