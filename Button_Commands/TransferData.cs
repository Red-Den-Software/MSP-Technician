using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Application = System.Windows.Application;
using Brushes = System.Windows.Media.Brushes;

namespace msptool.Button_Commands
{
    class TransferData
    {
        public void Transfer()
        {
            // Code to transfer data goes here
            
            
            var vm = new PopupViewModel
            {
                plogo = "↔",
                plogoForeground = Brushes.Orange,
                plogo_font = "60",
                Title = "Data Transfer",

                EllipseVisibility = System.Windows.Visibility.Collapsed,
                TitleBG = Brushes.Orange,
                errorText = "Transfer or Backup Data?",
                nVisibility = System.Windows.Visibility.Visible,
                YVisibility = System.Windows.Visibility.Visible,
                mVisibility = System.Windows.Visibility.Hidden,
                mbuttonthickness = "0",
                rbutText = "Transfer",
                lbutText = "Backup",
                
            };
            var popup = new Popup
            { 
                
                DataContext = vm
            };
            popup.Owner = Application.Current.MainWindow;
            popup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            popup.Show();


        }
        public void Backup()
        {
            
        }
    }
}
