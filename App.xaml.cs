using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Velopack;
namespace msptool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// using Velopack;
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // ✔ THIS is what Velopack needs to detect
            VelopackApp.Build().Run();

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }


    public partial class App : System.Windows.Application
    {
      public async Task CheckForUpdatesAsync()
        {
            var mgr = new UpdateManager("https://github.com/Red-Den-Software/MSP-Technician");

            var updates = await mgr.CheckForUpdatesAsync();

            if (updates != null)
            {
                await mgr.DownloadUpdatesAsync(updates);
                mgr.ApplyUpdatesAndRestart(updates, null);
            }
        }
    }

}