using Microsoft.Win32;
using msptool.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace msptool.Views
{
    public partial class CloneDiskView : System.Windows.Controls.UserControl
    {
       

        public CloneDiskView()
        {
           
            InitializeComponent();
            createButtons();

        }
        public List<string> GetDiskInfo()
        {
            List<string> drives = new List<string>();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady)
                {
                    drives.Add(d.Name);
                }
            }

            return drives;
        }
        public void createButtons()
        {
            List<string> driveLetters = GetDiskInfo();

            foreach (string drive in driveLetters)
            {
                System.Windows.Controls.Button driveButton = new System.Windows.Controls.Button();
                driveButton.Style = (Style)TryFindResource("cloneDiskBut");
                driveButton.Content = drive;
                driveButton.Height = 25;
                driveButton.Width = 296;

                ButtonPanelClone.Children.Add(driveButton);
            }
        }

    }

}
   
