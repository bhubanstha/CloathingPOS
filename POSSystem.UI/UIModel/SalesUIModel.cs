using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.UIModel
{
    public class SalesUIModel
    {
        public Bill Bill { get; set; }
        public List<Sales> Sales { get; set; }

    }
}
