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
        public class DriveItem
        {
            public string DisplayName { get; set; }
            public string RootPath { get; set; }
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
        
        public void CreateButtons()
        {
            List<DriveItem> driveItems = GetDiskInfo();

            foreach (DriveItem driveItem in driveItems)
            {
                System.Windows.Controls.RadioButton driveButton = new System.Windows.Controls.RadioButton();
                driveButton.GroupName = "Drives";
                driveButton.Content = driveItem.DisplayName;
                driveButton.Style = (Style)TryFindResource("cloneDiskBut");
                driveButton.Tag = driveItem.RootPath;
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
            DiskSelection.Instance.SelectedSourceDisk = rootPath;
            System.Diagnostics.Debug.WriteLine($"Selected Source Disk: {rootPath}");

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
   
