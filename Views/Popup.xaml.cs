using msptool.Button_Commands;
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

namespace msptool
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {


        public PopupWindow()
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
            TransferData transferData = new TransferData();
            transferData.Backup();
            System.Diagnostics.Debug.WriteLine("Backup button clicked");
        }
        
        private void rbutton(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
