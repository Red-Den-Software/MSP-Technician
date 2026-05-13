using Microsoft.VisualBasic;
using msptool.Commands;
using msptool.Commands.UpdateViewCommand;
using msptool.MVVM;
using msptool.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;


namespace msptool.Views
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
        private bool isOpen = false;
        private void HelpEllipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("OpenHelpAnimation");
            if (!isOpen)
            {
                storyboard.Begin(this); 
            }
            else
            {
                var closeStoryboard = (Storyboard)FindResource("CloseHelpAnimation");
                closeStoryboard.Begin(this);
            }
            isOpen = !isOpen;
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
