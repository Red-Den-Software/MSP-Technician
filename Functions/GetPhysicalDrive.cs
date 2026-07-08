using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace msptool.Functions
{
    public class GetPhysicalDrive
    {
        public static string GetPhysicalDriveFromLetter(string driveLetter)
        {
            using var logicalDisk =
                new ManagementObject($"Win32_LogicalDisk.DeviceID='{driveLetter}'");

            logicalDisk.Get();

            foreach (ManagementObject partition in logicalDisk.GetRelated("Win32_DiskPartition"))
            {
                foreach (ManagementObject disk in partition.GetRelated("Win32_DiskDrive"))
                {
                    driveLetter = disk["DeviceID"]?.ToString();
                    return disk["DeviceID"]?.ToString();
                }
            }

            return driveLetter;
        }
    }
}
