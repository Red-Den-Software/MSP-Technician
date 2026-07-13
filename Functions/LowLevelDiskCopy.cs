using Microsoft.Win32.SafeHandles;
using msptool.Button_Commands;
using msptool.Commands.UpdateViewCommand;
using msptool.MVVM;
using msptool.ViewModels;
using msptool.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace msptool.Functions
{
    public class LowLevelDiskCopy
    {
        ShellViewModel _parent;

        public class DriveItem
        {
            public string DisplayName { get; set; }
            public string RootPath { get; set; }
        }
        public static List<DriveItem> GetDiskInfo()
        {
            List<DriveItem> drives = new List<DriveItem>();

            SelectQuery query = new SelectQuery("SELECT * FROM Win32_DiskDrive");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject drive in searcher.Get())
                {
                    bool foundLogicalDisk = false;
                    string model = drive["Model"]?.ToString();
                    string brand = GetBrand(model);
                    string deviceId = drive["DeviceID"]?.ToString();
                    if (!string.IsNullOrEmpty(deviceId))
                    {
                        using (ManagementObjectSearcher partitionSearcher = new ManagementObjectSearcher(
                            $"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{deviceId}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition"))
                        {
                            foreach (ManagementObject partition in partitionSearcher.Get())
                            {
                                using (ManagementObjectSearcher logicalSearcher = new ManagementObjectSearcher(
                                    $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass = Win32_LogicalDiskToPartition"))
                                {
                                    foreach (ManagementObject logical in logicalSearcher.Get())
                                    {
                                        foundLogicalDisk = true;
                                        string rootPath = logical["DeviceID"]?.ToString() + "\\";
                                        drives.Add(new DriveItem
                                        {
                                            DisplayName = $"{brand} {model} ({rootPath})",
                                            RootPath = rootPath
                                        });
                                    }
                                }
                            }
                        }
                    }
                    if (!foundLogicalDisk)
                    {
                        drives.Add(new DriveItem
                        {
                            DisplayName = $"{brand} {model} Unallocated",
                            RootPath = deviceId
                        });
                    }
                }
            }
            return drives;
        }
        private static string GetBrand(string model)
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
        public LowLevelDiskCopy()
        {

        }
        const uint GENERIC_READ = 0x80000000;
        const uint GENERIC_WRITE = 0x40000000;
        const uint FILE_SHARE_READ = 0x00000001;
        const uint FILE_SHARE_WRITE = 0x00000002;
        const uint OPEN_EXISTING = 3;
        const uint FILE_FLAG_NO_BUFFERING = 0x20000000;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(
            SafeFileHandle hFile,
            IntPtr lpBuffer,
            uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(
            SafeFileHandle hFile,
            IntPtr lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetFileSizeEx(
        SafeFileHandle hFile,
        out long lpFileSize);
        // [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        // public static extern bool DeviceIoControl(...) -> For Lock/Dismount

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool DeviceIoControl(
    SafeFileHandle hDevice,
    uint dwIoControlCode,
    IntPtr lpInBuffer,
    uint nInBufferSize,
    out GET_LENGTH_INFORMATION lpOutBuffer,
    uint nOutBufferSize,
    out uint lpBytesReturned,
    IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool DeviceIoControlDismount(
       SafeFileHandle hDevice,
        uint dwIoControlCode,
        IntPtr lpInBuffer,
        uint nInBufferSize,
        IntPtr lpOutBuffer,
        uint nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

        [StructLayout(LayoutKind.Sequential)]
        public struct GET_LENGTH_INFORMATION
        {
            public long Length;
        }
        public void CopySectorsWithProgress(IProgress<CopyProgress> progress)
        {
            Task.Run(() => CopySectors(progress));
        }

        public static void CopySectors(IProgress<CopyProgress> progress)
        {
            // Target physical drive 0 (Ensure this maps to Drive C: using IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS first)
            const uint IOCTL_DISK_GET_LENGTH_INFO = 0x0007405C;
            const uint FSCTL_LOCK_VOLUME = 0x00090018;
            const uint FSCTL_DISMOUNT_VOLUME = 0x00090020;
            bool success;
            GET_LENGTH_INFORMATION lengthInfo;
            uint bytesReturned;

            string destinationDrive = GetPhysicalDrive.GetPhysicalDriveFromLetter(DiskSelection.Instance.SelectedDestinationDisk);
            string sourceDrive = GetPhysicalDrive.GetPhysicalDriveFromLetter(DiskSelection.Instance.SelectedSourceDisk);

            SafeFileHandle hSource = CreateFile(sourceDrive, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING, IntPtr.Zero);
            SafeFileHandle hDest = CreateFile(destinationDrive, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING, IntPtr.Zero);

            if (hSource.IsInvalid || hDest.IsInvalid)
            {
                 if (hSource.IsInvalid)
                {
                    DialogResult result = MessageBox.Show("Failed to open source drive handle. Ensure app runs as Admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                       
                        return;
                    }
                }
                if (hDest.IsInvalid)
                {
                    MessageBox.Show($" {destinationDrive} Failed to open destination drive handle. Ensure app runs as Admin.\nError: {Marshal.GetLastWin32Error()}");
                    DialogResult result = MessageBox.Show("Failed to open destination drive handle. Ensure app runs as Admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                       
                        return;
                    }
                }
                return;
            }
            string volumePath =
            @"\\.\" + DiskSelection.Instance.SelectedDestinationDisk.TrimEnd('\\');

            SafeFileHandle hVolume = CreateFile(
            volumePath,
            GENERIC_READ | GENERIC_WRITE,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            IntPtr.Zero,
            OPEN_EXISTING,
            0,
            IntPtr.Zero);

            if (hVolume.IsInvalid)
            {
                MessageBox.Show(
                    $"Unable to open volume.\n{Marshal.GetLastWin32Error()}");
                return;
            }
            if (!DeviceIoControlDismount(
        hVolume,
        FSCTL_DISMOUNT_VOLUME,
        IntPtr.Zero,
        0,
        IntPtr.Zero,
        0,
        out bytesReturned,
        IntPtr.Zero))
            {
                MessageBox.Show(
                    $"Unable to lock volume.\nError {Marshal.GetLastWin32Error()}");
            }

            if (!DeviceIoControl(
                hSource,
                IOCTL_DISK_GET_LENGTH_INFO,
                IntPtr.Zero,
                0,
                out lengthInfo,
                (uint)Marshal.SizeOf<GET_LENGTH_INFORMATION>(),
                out bytesReturned,
                 IntPtr.Zero))
            {
                MessageBox.Show(
                    $"Unable to determine disk size.\nError: {Marshal.GetLastWin32Error()}");
                return;
            }
            long totalBytes = lengthInfo.Length;


            //Adjusted from 4096 to 4MB
            uint bufferSize = 4 * 1024 * 1024;
            IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);

            try
            {
                uint bytesRead;
                uint bytesWritten;
                long bytesCopied = 0;


                while ((success = ReadFile(
                hSource,
                buffer,
                bufferSize,
                out bytesRead,
                IntPtr.Zero)) && bytesRead > 0)
                {



                    if (!WriteFile(hDest, buffer, bytesRead, out bytesWritten, IntPtr.Zero))
                    {
                        MessageBox.Show($"WriteFile failed: {Marshal.GetLastWin32Error()}");
                        break;
                    }
                    if (bytesRead != bytesWritten)
                    {
                        MessageBox.Show("Write error occurred.");
                        break;
                    }
                    bytesCopied += bytesWritten;

                    progress?.Report(new CopyProgress
                    {
                        BytesCopied = bytesCopied,
                        TotalBytes = totalBytes
                    });
                }
            }

            finally
            {
                uint signature = (uint)Random.Shared.NextInt64(1, uint.MaxValue);
                Marshal.FreeHGlobal(buffer);
                hSource.Close();
                ChangeDiskSignature(DiskSelection.Instance.SelectedDestinationDisk, signature); // Example new signature
                hDest.Close();
                hVolume.Close();
            }
            if (!success)
            {
                MessageBox.Show($"ReadFile failed: {Marshal.GetLastWin32Error()}");
            }
        }



        public static void ChangeDiskSignature(string driveLetter, uint newSignature)
        {
            string physicalDrive = GetPhysicalDrive.GetPhysicalDriveFromLetter(driveLetter);
            SafeFileHandle hDrive = CreateFile(physicalDrive, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            if (hDrive.IsInvalid)
            {
                MessageBox.Show($"Failed to open drive handle for {driveLetter}. Ensure app runs as Admin.");
                
                return;
            }
            // Read the MBR
            byte[] mbr = new byte[512];
            IntPtr buffer = Marshal.AllocHGlobal(mbr.Length);
            try
            {
                uint bytesRead;
                if (!ReadFile(hDrive, buffer, (uint)mbr.Length, out bytesRead, IntPtr.Zero) || bytesRead != mbr.Length)
                {
                    MessageBox.Show($"Failed to read MBR from {driveLetter}. Error: {Marshal.GetLastWin32Error()}");
                    return;
                }
                Marshal.Copy(buffer, mbr, 0, mbr.Length);
                // Change the disk signature (bytes 440-443)
                BitConverter.GetBytes(newSignature).CopyTo(mbr, 440);
                // Write the modified MBR back
                Marshal.Copy(mbr, 0, buffer, mbr.Length);
                uint bytesWritten;
                if (!WriteFile(hDrive, buffer, (uint)mbr.Length, out bytesWritten, IntPtr.Zero) || bytesWritten != mbr.Length)
                {
                    MessageBox.Show($"Failed to write modified MBR to {driveLetter}. Error: {Marshal.GetLastWin32Error()}");
                    return;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
                hDrive.Close();
            }
        }
        public class CopyProgress
        {
            public long BytesCopied { get; set; }
            public long TotalBytes { get; set; }
            public double Percentage => TotalBytes > 0 ? (double)BytesCopied / TotalBytes * 100 : 0;
        }
    }
}
