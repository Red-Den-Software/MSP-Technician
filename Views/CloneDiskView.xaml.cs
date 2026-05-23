using Microsoft.Win32;
using msptool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace msptool.Views
{
    public partial class CloneDiskView : System.Windows.Controls.UserControl
    {
        public CloneDiskView()
        {
            InitializeComponent();
        }
        private string _srcdisk;
      
        public string source_disk { get => source_disk; set { _srcdisk = value; } }
        private void setsource_click(object sender, RoutedEventArgs e)
        {
            
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            dialog.SelectedPath = _srcdisk;
        }
        private string Srcdisk { get => _srcdisk; set { _srcdisk = value; } }

    }
   
}
