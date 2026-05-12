using msptool.Views;
using System.Threading.Tasks;
using System.Windows;
using Velopack;

namespace msptool
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            VelopackApp.Build().Run();

            base.OnStartup(e);
           

            var window = new ShellView();
            window.Show();
        }

        public async Task CheckForUpdatesAsync()
        {
            var mgr = new UpdateManager(
                "https://github.com/Red-Den-Software/MSP-Technician"
            );

            var updates = await mgr.CheckForUpdatesAsync();

            if (updates != null)
            {
                await mgr.DownloadUpdatesAsync(updates);
                mgr.ApplyUpdatesAndRestart(updates, null);
            }
        }
    }
}