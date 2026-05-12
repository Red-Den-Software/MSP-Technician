using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Transfer_App.ViewModels.Usercontrols
{
    class TextBoxViewModel
    {
        
    }
    class DirectoryCreation
    {
        

        private string Todaysdate => DateTime.Now.ToString("MMddyy") + " - " + string.Empty;
        private string path
        {
            get
            {
                // Get the root directory of the current drive
                return System.IO.Directory.GetDirectoryRoot(Environment.CurrentDirectory);
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        public void createDire()
        {

            Trace.WriteLine($"Creating directory with name: {Todaysdate}");
            // Ensure 'path' is not null to avoid CS8602
            if (string.IsNullOrEmpty(path))
            {
                string cderrorMessage = "Path cannot be null or empty.";
                Trace.WriteLine("Path cannot be determined.");

            }
            Directory.SetCurrentDirectory(path);
            if (Directory.Exists(Todaysdate))
            {

                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show($"Are you sure you want to overwrite the folder {Todaysdate} at {path}?", $"Folder already exists!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Directory.CreateDirectory(Todaysdate);

                }
                else if (dialogResult == DialogResult.No)
                {
                    Trace.WriteLine("Folder not overwritten, returning");

                }



            }
            else
            {
                string noanswer = "No answer";
            }

        }
    }

}
