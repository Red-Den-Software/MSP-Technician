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
        IntPtr lpOutBuffer,
        uint nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

        [StructLayout(LayoutKind.Sequential)]
        public struct GET_LENGTH_INFORMATION
        {
            public long Length;
        }
        public static void CopySectors(IProgress<CopyProgress> progress)
        {
            // Target physical drive 0 (Ensure this maps to Drive C: using IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS first)
            const uint IOCTL_DISK_GET_LENGTH_INFO = 0x0007405C;
            const uint FSCTL_LOCK_VOLUME = 0x00090018;
            const uint FSCTL_DISMOUNT_VOLUME = 0x00090020;

            GET_LENGTH_INFORMATION lengthInfo;
            uint bytesReturned;

            string destinationDrive = GetPhysicalDrive.GetPhysicalDriveFromLetter(DiskSelection.Instance.SelectedDestinationDisk);
            string sourceDrive = GetPhysicalDrive.GetPhysicalDriveFromLetter(DiskSelection.Instance.SelectedSourceDisk);
           
            SafeFileHandle hSource = CreateFile(sourceDrive, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING, IntPtr.Zero);
            SafeFileHandle hDest = CreateFile(destinationDrive, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING, IntPtr.Zero);

            if (hSource.IsInvalid || hDest.IsInvalid)
            {
                MessageBox.Show("Failed to open drive handles. Ensure app runs as Admin.");
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
            if (!DeviceIoControl(
                    hVolume,
                    FSCTL_LOCK_VOLUME,
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
                

                while (ReadFile(hSource, buffer, bufferSize, out bytesRead, IntPtr.Zero) && bytesRead > 0)
                {

                    WriteFile(hDest, buffer, bytesRead, out bytesWritten, IntPtr.Zero);

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
                Marshal.FreeHGlobal(buffer);
                hSource.Close();
                hDest.Close();
            }
        }

    }
    public class  CopyProgress
    {
        public long BytesCopied { get; set; }
        public long TotalBytes { get; set; }
        public double Percentage => TotalBytes > 0 ? (double)BytesCopied / TotalBytes * 100 : 0;
    }
}
