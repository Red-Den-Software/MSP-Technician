using Data_Transfer_App.MVVM;
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
using System.Windows.Shapes;
using Data_Transfer_App.ViewModels;
using Data_Transfer_App.Commands;
using MessageBox = System.Windows.Forms.MessageBox;
using System.ComponentModel;
using Data_Transfer_App.Commands.UpdateViewCommand;
using Microsoft.VisualBasic;


namespace Data_Transfer_App.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        

        public ShellView()
        {
            InitializeComponent();
            DataContext = new ShellViewModel();
        }
       
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();


        }

        private void toBtn_Click(object sender)
        {

        }
    }
}
