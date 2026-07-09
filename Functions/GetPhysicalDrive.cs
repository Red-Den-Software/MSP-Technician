using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace msptool.Functions
{
    public class GetPhysicalDrive
    {
        public static string GetPhysicalDriveFromLetter(string driveLetter)
        {
            driveLetter = driveLetter.TrimEnd('\\');

            using var logicalDisk =
                new ManagementObject($"Win32_LogicalDisk.DeviceID='{driveLetter}'");

            logicalDisk.Get();

            foreach (ManagementObject partition in logicalDisk.GetRelated("Win32_DiskPartition"))
            {
                foreach (ManagementObject disk in partition.GetRelated("Win32_DiskDrive"))
                {
                    return disk["DeviceID"]?.ToString();
                }
            }
            return null;


        }
    }     
        }