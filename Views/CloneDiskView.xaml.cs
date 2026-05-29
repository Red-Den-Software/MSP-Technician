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
    public partial class CloneDiskView : System.Windows.Controls.UserControl
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public CloneDiskView()
        {
           
            InitializeComponent();
            CreateButtons();

        }
        private string _src_disk;
        private CloneDiskView _parentView;

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
        public string SelectedSourceDisk
        {
            get => _src_disk;
            set
            {
                _src_disk = value;
                OnPropertyChanged(nameof(SelectedSourceDisk));
            }
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
            System.Windows.Controls.RadioButton button = sender as RadioButton;

            string drive = button.Tag.ToString();

            var vm = DataContext as CloneDiskViewModelPg2;
            if (vm == null)
            {
                MessageBox.Show("ViewModel is null");
                return;
            }
            vm.SourceDisk = drive;
            
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
   
