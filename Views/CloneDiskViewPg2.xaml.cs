using Microsoft.Win32;
using msptool.Button_Commands;
using msptool.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using static msptool.Views.CloneDiskView;
using MessageBox = System.Windows.Forms.MessageBox;
using RadioButton = System.Windows.Controls.RadioButton;
namespace msptool.Views
{
    public partial class CloneDiskViewPg2 : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
     

        public CloneDiskViewPg2()
        {
            
            InitializeComponent();
            this.Loaded += CloneDiskViewPg2_Loaded;

        }
        private void CloneDiskViewPg2_Loaded(object sender, RoutedEventArgs e)
        {
            // Unhook the event so it only runs once
            this.Loaded -= CloneDiskViewPg2_Loaded;

            // Now it is safe to build your UI elements
            CreateButtons();
        }
        private CloneDiskView _cloneDiskView;
        private ShellViewModel ShellViewModel;
        private CloneDiskViewModelPg2 _cloneDiskViewModelPg2;
        public void CreateButtons()
        {
            // 1. Safe check: Ensure the WPF panel exists in memory
            if (ButtonPanelCloneDest == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: ButtonPanelClone XAML element is null.");
                return;
            }

            // Clear any old buttons if the page reloads
            ButtonPanelCloneDest.Children.Clear();

            // 2. Fetch disk info
            List<DriveItem> driveLetters = GetDiskInfo();

            // 3. Safe check: Ensure GetDiskInfo didn't return a null object
            if (driveLetters == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: GetDiskInfo() returned null.");
                return;
            }

            // 4. GET THE SELECTED SOURCE DISK VALUE
            string sourceDisk = null;
            var mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow != null && mainWindow.DataContext is ShellViewModel shellVM)
            {
                sourceDisk = DiskSelection.Instance.RootSourceDisk;
            }

            // 5. Build the buttons safely
            foreach (DriveItem driveItem in driveLetters)
            {
                if (string.IsNullOrEmpty(driveItem.RootPath)) continue;

                // EXCLUDE CHECK: Skip this drive if it matches the selected source disk
                // StringComparison removes issues with casing (e.g., "C:\" vs "c:\")
                if (!string.IsNullOrEmpty(sourceDisk) &&
                    driveItem.RootPath.Equals(sourceDisk, StringComparison.OrdinalIgnoreCase))
                {
                    continue; // Skip creating a button for this drive
                }

                System.Windows.Controls.RadioButton driveButton = new System.Windows.Controls.RadioButton();
                driveButton.GroupName = "Drives";
                driveButton.Content = driveItem.DisplayName;


                driveButton.Style = (Style)TryFindResource("cloneDiskBut");
                driveButton.Tag = driveItem.RootPath;
                driveButton.Click += DriveButton_Click;

                ButtonPanelCloneDest.Children.Add(driveButton);
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
            DiskSelection.Instance.SelectedDestinationDisk = rootPath;
            System.Diagnostics.Debug.WriteLine($"Selected Destination Disk: {rootPath}");


        }
        public List<DriveItem> GetDiskInfo()
        {
            List<DriveItem> drives = new List<DriveItem>();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady)
                {

                    string model = GetDriveModel(d.Name);

                    string brand = GetBrand(model);

                    drives.Add(new DriveItem
                    {
                        DisplayName = $"{brand} {model} ({d.Name})",
                        RootPath = d.Name
                    });
                }
            }

            return drives;
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
   
