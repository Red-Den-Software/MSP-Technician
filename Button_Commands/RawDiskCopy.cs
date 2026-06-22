using Microsoft.Win32.SafeHandles;
using msptool.ViewModels;
using msptool.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace msptool.Button_Commands
{
    class RawDiskCopier
    {
        // Win32 Imports for Direct Disk Access
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
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        // Constants
        const uint GENERIC_READ = 0x80000000;
        const uint GENERIC_WRITE = 0x40000000;
        const uint FILE_SHARE_READ = 0x00000001;
        const uint FILE_SHARE_WRITE = 0x00000002;
        const uint OPEN_EXISTING = 3;
        const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        const uint FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000;
        const uint FSCTL_LOCK_VOLUME = 0x00090018;
        const uint FSCTL_UNLOCK_VOLUME = 0x0009001C;
        const uint FSCTL_DISMOUNT_VOLUME = 0x00090020;

       
        public void CloneDisk(string sourceDrive, string destDrive, int bufferSizeInMb = 8)
        {
            int bufferSize = bufferSizeInMb * 1024 * 1024; // e.g., 8MB chunks
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);

            try
            {
                // 1. Open Source Drive
                SafeFileHandle hSource = CreateFile(sourceDrive, GENERIC_READ, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING | FILE_FLAG_SEQUENTIAL_SCAN, IntPtr.Zero);
                MessageBox.Show("Source Drive Opened: " + sourceDrive + hSource);
                if (hSource.IsInvalid) throw new Exception("Failed to open source drive. Error: " + Marshal.GetLastWin32Error());

                // 2. Open Destination Drive
                SafeFileHandle hDest = CreateFile(destDrive, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING | FILE_FLAG_SEQUENTIAL_SCAN, IntPtr.Zero);
                MessageBox.Show("Destination Drive Opened: " + destDrive + hDest);
                if (hDest.IsInvalid) throw new Exception("Failed to open destination drive. Error: " + Marshal.GetLastWin32Error());

                // 3. Lock and Dismount volumes to prevent OS interference (Recommended for partitions)
                uint bytesReturned;
                DeviceIoControl(hSource, FSCTL_LOCK_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);
                DeviceIoControl(hDest, FSCTL_DISMOUNT_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

                // 4. Perform Sector-by-Sector Copy
                using (FileStream fsSource = new FileStream(hSource, FileAccess.Read, bufferSize))
                using (FileStream fsDest = new FileStream(hDest, FileAccess.Write, bufferSize))
                {
                    byte[] managedBuffer = new byte[bufferSize];
                    int bytesRead;

                    while ((bytesRead = fsSource.Read(managedBuffer, 0, managedBuffer.Length)) > 0)
                    {
                        fsDest.Write(managedBuffer, 0, bytesRead);
                        Views.CloneDiskFinal viewmodel = new Views.CloneDiskFinal();
                        viewmodel.UpdateProgress((int)fsSource.Position, (int)fsSource.Length);
                        
                        
                       
                    }
                    
                   
                }

                // 5. Unlock volumes
                DeviceIoControl(hSource, FSCTL_UNLOCK_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);
                Debug.WriteLine("\nClone completed successfully!");
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static void Main()
        {
            // Use exact paths like @"\\.\PhysicalDrive0" and @"\\.\PhysicalDrive1"
            // Note: Running this application as Administrator is required.
            string source = @"\\.\PhysicalDrive0";
            string dest = @"\\.\PhysicalDrive1";

            // CloneDisk(source, dest);
        }
    }

}
