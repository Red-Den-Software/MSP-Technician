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
       
      
       
        private void setsource_click(object sender, RoutedEventArgs e)
        {
            ((CloneDiskViewModel)DataContext).src_disk();

        }

        private void Start_Clone_Click(object sender, RoutedEventArgs e)
        {

        }

        private void setdest_click(object sender, RoutedEventArgs e)
        {
            ((CloneDiskViewModel)DataContext).dst_disk();
        }
    }

}
   
