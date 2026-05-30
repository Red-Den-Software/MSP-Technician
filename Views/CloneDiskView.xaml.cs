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
using MessageBox = System.Windows.Forms.MessageBox;
using RadioButton = System.Windows.Controls.RadioButton;
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
        public List<string> GetDiskInfo()
        {
            List<string> drives = new List<string>();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady)
                {
                    string model = GetDriveModel(d.Name);

                    string brand = GetBrand(model);

                    drives.Add($"{brand} {model} ({d.Name})");
                }
            }

            return drives;
        }
        
        public void CreateButtons()
        {
            List<string> driveLetters = GetDiskInfo();

            foreach (string drive in driveLetters)
            {
                System.Windows.Controls.RadioButton driveButton = new System.Windows.Controls.RadioButton();
               driveButton.GroupName = "Drives";
                driveButton.Content = drive;
                driveButton.Style = (Style)TryFindResource("cloneDiskBut");
                driveButton.Tag = drive;
                driveButton.Click += DriveButton_Click;
                ButtonPanelClone.Children.Add(driveButton);
            }
        }
        private void DriveButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Ensure the sender is actually a RadioButton
            if (sender is RadioButton button)
            {
                // 2. Ensure Tag is not null before converting to string
                string driveLetter = button.Tag?.ToString();
                if (string.IsNullOrEmpty(driveLetter))
                {
                    return; // Exit early if there is no drive letter data
                }

                // 3. Defensive Check for Solution 1 (Constructor passing)
                if (_shellVM != null)
                {
                    _shellVM.SelectedSourceDisk = driveLetter;
                    return; // Success!
                }

                // 4. Defensive Check for Solution 2 (Window DataContext lookup)
                var mainWindow = System.Windows.Application.Current.MainWindow;
                if (mainWindow != null && mainWindow.DataContext is ShellViewModel shellVM)
                {
                    shellVM.SelectedSourceDisk = driveLetter;
                    return; // Success!
                }

                // 5. Fallback: Check if the current View's own DataContext can find it
                if (this.DataContext is ShellViewModel alternativeShellVM)
                {
                    alternativeShellVM.SelectedSourceDisk = driveLetter;
                    return; // Success!
                }

                // If it reaches here, the application cannot locate the active ShellViewModel instance
                System.Diagnostics.Debug.WriteLine("Error: ShellViewModel instance could not be found anywhere.");
            }
        }

        private string GetDriveModel(string driveLetter)
        {
            try
            {
                string letter = driveLetter.Replace("\\", "");

                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                        "SELECT * FROM Win32_LogicalDiskToPartition"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string dependent = obj["Dependent"].ToString();

                        if (dependent.Contains(letter))
                        {
                            string antecedent = obj["Antecedent"].ToString();

                            string partitionId = antecedent.Split('"')[1];

                            using (ManagementObjectSearcher diskSearcher =
                                new ManagementObjectSearcher(
                                    "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" +
                                    partitionId +
                                    "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition"))
                            {
                                foreach (ManagementObject disk in diskSearcher.Get())
                                {
                                    return disk["Model"]?.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return "Unknown Drive";
        }
        private string GetBrand(string model)
        {
            if (model.StartsWith("CT"))
                return "Crucial";

            if (model.StartsWith("WDC"))
                return "Western Digital";

            if (model.StartsWith("Samsung"))
                return "Samsung";

            if (model.StartsWith("ST"))
                return "Seagate";

            if (model.StartsWith("KINGSTON"))
                return "Kingston";

            if (model.StartsWith("SanDisk"))
                return "SanDisk";

            return "Unknown";
        }
    }

}
   
