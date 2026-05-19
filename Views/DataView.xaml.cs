using Microsoft.Win32;
using msptool.Commands;
using msptool.MVVM;
using msptool.ViewModels;
using msptool.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace msptool.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class DataView : System.Windows.Controls.UserControl
    {


        public DataView()
        {
            InitializeComponent();
            this.DataContext = new DataViewModel();

        }
        public string Todaysdate
        {
            get
            {
                return DateTime.Now.ToString(@"MM\/dd\/yy");

            }
        }
        public bool IsTextBoxEmpty { get; set;  }
        
        public void tad_Click(object sender, RoutedEventArgs e)
        {
           
            if (!IsTextBoxEmpty)
            {
                var folderDialog = new OpenFolderDialog()
                {
                    Title = "Select Folder",
                    InitialDirectory = @"C:\Users",
                    Multiselect = true,

                };
                var folderName = folderDialog.FolderName;
                System.Windows.MessageBox.Show($"You picked {folderName}!");
                string[] files = System.IO.Directory.GetFiles(folderName, "*.*", System.IO.SearchOption.AllDirectories);
                string destinationPath = @"C:\";
                if (File.Exists(destinationPath))
                {
                    Trace.WriteLine("File already exists at the destination path.");
                    return;
                }
                else
                {
                    Directory.CreateDirectory(destinationPath);
                    Trace.WriteLine("Directory created at the destination path.");
                }
            }
            




        }
        
        
        private void wifi_transfer(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow();
            PopupViewModel vm = new PopupViewModel();
            vm.TitleBG = System.Windows.Media.Brushes.DarkOrange;
            popup.DataContext = vm;

            vm.plogo = "↔";
            vm.plogoForeground = System.Windows.Media.Brushes.DarkOrange;
            vm.EllipseVisibility = Visibility.Visible;
            vm.EllipseStroke = System.Windows.Media.Brushes.DarkOrange;
            vm.YVisibility = Visibility.Visible;
            vm.Title = "Wi-Fi Transfer";
           
                Trace.WriteLine("TextBox is empty, please enter a name.");
                vm.TitleBG = System.Windows.Media.Brushes.Red;
                popup.DataContext = vm;
                vm.plogo = "X";
                vm.plogoForeground = System.Windows.Media.Brushes.Red;
                vm.EllipseVisibility = Visibility.Visible;
                vm.EllipseStroke = System.Windows.Media.Brushes.Red;
                popup.Owner = System.Windows.Application.Current.MainWindow;
                popup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                vm.errorText = "Customer Name Box is empty, please enter a name.";
                popup.Show();
                return;
            
            if (vm.YVisibility == Visibility.Visible)
            {
                vm.nVisibility = Visibility.Visible;
                vm.mVisibility = Visibility.Collapsed;
                vm.mbuttonthickness = "0";
                vm.rbuttonthickness = "2";
                vm.rbutText = "Transfer";
                vm.lbutText = "Backup";
                vm.lbuttonthickness = "2";
            }

            popup.Owner = System.Windows.Application.Current.MainWindow;
            popup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            vm.errorText = "Would you like to transfer data via Wi-Fi?";
            popup.Show();
        }

        private void clonedisk_button(object sender, RoutedEventArgs e)
        {
            
           
        }
    }
}
