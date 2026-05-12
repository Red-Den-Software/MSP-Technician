using Caliburn.Micro;
using Data_Transfer_App.ViewModels;
using Data_Transfer_App.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
namespace Data_Transfer_App
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
                Initialize();

        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync(typeof(ShellViewModel));
        }
      


    }
}
