using msptool;
using System;
using System.Windows;
using Velopack;

namespace Data_Transfer_App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // ✔ REQUIRED for Velopack detection
            VelopackApp.Build().Run();

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}