using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Data_Transfer_App
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {


        public Popup()
        {
            InitializeComponent();
            DataContext = new PopupViewModel();

        }

        private void okBtn(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        private void lbutton(object sender, RoutedEventArgs e)
        {

        }
        private void rbutton(object sender, RoutedEventArgs e)
        {

        }
    }
}
