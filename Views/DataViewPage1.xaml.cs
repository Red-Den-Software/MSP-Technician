using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using msptool.ViewModels;
namespace msptool.Views
{
    /// <summary>
    /// Interaction logic for DataViewPage1.xaml
    /// </summary>
    public partial class DataViewPage1 : System.Windows.Controls.UserControl
    {
        public DataViewPage1()
        {
           InitializeComponent();
            DataContext = new DataViewModelPage1();
        }
    }
}
