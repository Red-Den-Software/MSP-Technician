using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msptool.ViewModels
{
    public class DataViewModelPage1
    {
        private readonly DataViewModel _parent;
        public DataViewModelPage1(Views.CloneDiskView cloneDiskView)
        {
        }

        public DataViewModelPage1(DataViewModel parent)
        {
            _parent = parent;
        }

        public void SendResult(string value)
        {
            _parent.HandleFromPage1(value);
        }
    }
}
   
