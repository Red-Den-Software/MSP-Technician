using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace msptool.Button_Commands
{
    class DiskCheck
    {
        private static readonly DriveType _instancedrivetype = new();
        public static DriveType Instance => _instancedrivetype;
        public enum DriveType
        {
            Unknown,
            NoRootDirectory,
            Removable,
            Fixed,
            Network,
            CDRom,
            Ram
        }

        public void CheckDiskType(string driveLetter)
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(driveLetter);
                DriveType driveType = (DriveType)driveInfo.DriveType;
                switch (driveType)
                {
                    case DriveType.Removable:
                        System.Windows.MessageBox.Show($"{driveLetter} is a removable drive.");
                        break;
                    case DriveType.Fixed:
                        System.Windows.MessageBox.Show($"{driveLetter} is a fixed drive.");
                        break;
                    case DriveType.Network:
                        System.Windows.MessageBox.Show($"{driveLetter} is a network drive.");
                        break;
                    case DriveType.CDRom:
                        System.Windows. MessageBox.Show($"{driveLetter} is a CD-ROM drive.");
                        break;
                    case DriveType.Ram:
                        System.Windows.MessageBox.Show($"{driveLetter} is a RAM disk.");
                        break;
                    default:
                        System.Windows.MessageBox.Show($"{driveLetter} is of unknown type.");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error checking disk type: {ex.Message}");
            }
        }

    }
}
