using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Management;

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
                        Debug.WriteLine($"{driveLetter} is a removable drive.");
                        break;
                    case DriveType.Fixed:
                        Debug.WriteLine($"{driveLetter} is a fixed drive.");
                        break;
                    case DriveType.Network:
                        Debug.WriteLine($"{driveLetter} is a network drive.");
                        break;
                    case DriveType.CDRom:
                        Debug.WriteLine($"{driveLetter} is a CD-ROM drive.");
                        break;
                    case DriveType.Ram:
                        Debug.WriteLine($"{driveLetter} is a RAM disk.");
                        break;
                    default:
                        Debug.WriteLine($"{driveLetter} is of unknown type.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking disk type: {ex.Message}");
            }
            try
            {
                // 1. Get the partition associated with the logical disk
                string partitionQuery = $"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{driveLetter}'}} WHERE AssocClass = Win32_LogicalDiskToPartition";
                using var partitionSearcher = new ManagementObjectSearcher(partitionQuery);

                foreach (ManagementObject partition in partitionSearcher.Get())
                {
                    // 2. Get the physical disk drive associated with that partition
                    string diskQuery = $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
                    using var diskSearcher = new ManagementObjectSearcher(diskQuery);

                    foreach (ManagementObject disk in diskSearcher.Get())
                    {
                       
                        
                         
                    }
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine($"WMI Query Error: {e.Message}");
            }
        }

    }
}
