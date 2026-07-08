using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Security.RightsManagement;
using msptool.ViewModels;

namespace msptool.Functions
{
    public class LowLevelDiskCopy
    {
      public LowLevelDiskCopy()
        {
            MessageBox.Show("This function is not yet implemented. Please use the CloneDiskFinal view for disk cloning.");
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

        // [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        // public static extern bool DeviceIoControl(...) -> For Lock/Dismount

        public static void CopySectors()
        {
            // Target physical drive 0 (Ensure this maps to Drive C: using IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS first)
            string sourceDrive = GetPhysicalDrive.GetPhysicalDriveFromLetter(DiskSelection.Instance.SelectedSourceDisk);
            string destinationDrive = GetPhysicalDrive.GetPhysicalDriveFromLetter(DiskSelection.Instance.SelectedDestinationDisk);

            SafeFileHandle hSource = CreateFile(sourceDrive, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING, IntPtr.Zero);
            SafeFileHandle hDest = CreateFile(destinationDrive, GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING, IntPtr.Zero);

            if (hSource.IsInvalid || hDest.IsInvalid)
            {
                Console.WriteLine("Failed to open drive handles. Ensure app runs as Admin.");
                return;
            }

            // Must align with the drive's physical sector size, typically 4096 or 512 bytes
            uint bufferSize = 4096;
            IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);

            try
            {
                uint bytesRead;
                uint bytesWritten;

                while (ReadFile(hSource, buffer, bufferSize, out bytesRead, IntPtr.Zero) && bytesRead > 0)
                {
                    WriteFile(hDest, buffer, bytesRead, out bytesWritten, IntPtr.Zero);
                    if (bytesRead != bytesWritten)
                    {
                        Console.WriteLine("Write error occurred.");
                        break;
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
                hSource.Close();
                hDest.Close();
            }
        }

    }
}
