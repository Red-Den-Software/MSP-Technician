using System;
using System.Collections.Generic;
using System.IO;
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
        public static class PopupManager
        {
            public static PopupWindow? CurrentPopup { get; set; }
        }
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
            var popup = new PopupWindow
            { 
                
                DataContext = vm
            };
            popup.Owner = Application.Current.MainWindow;
            PopupManager.CurrentPopup = popup;
            popup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            popup.Show();

            
        }
        private string backupPath = string.Empty; // Example backup path

        public string BackupPath { get => backupPath; set => backupPath = value; }
        public void closePopup()
        {
            PopupManager.CurrentPopup?.Close();
            PopupManager.CurrentPopup = null;
        }

        public void Backup()
        {
            closePopup();
            var vm = new PopupViewModel
            {
                plogo = "↔",
                plogoForeground = Brushes.Orange,
                plogo_font = "60",
                Title = "Data Transfer",

                EllipseVisibility = System.Windows.Visibility.Collapsed,
                TitleBG = Brushes.Orange,
                errorText = $"Is this path correct?",
               // centerInlineText = $"{BackupPath}",
                nVisibility = System.Windows.Visibility.Visible,
                YVisibility = System.Windows.Visibility.Visible,
                mVisibility = System.Windows.Visibility.Hidden,
                mbuttonthickness = "0",
                rbutText = "No",
                lbutText = "Yes",

            };
            var popup = new PopupWindow
            {

                DataContext = vm
            };
            popup.Show();

            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] profileFolders = Directory.GetDirectories(rootPath);
            foreach (string folder in profileFolders)
            {
                vm.pathText = folder;
                vm.pathComboBox.Items.Add(folder);
            }
           
                
               
            
        }
    }
}
