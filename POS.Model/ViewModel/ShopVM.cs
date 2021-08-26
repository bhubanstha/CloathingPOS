using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model.ViewModel
{
    public class ShopVM 
    {

        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string PANNumber { get; set; }
        public string LogoPath { get; set; }
        public bool CalculateVATOnSales { get; set; }
        public bool PrintInvoice { get; set; }
        public string PdfPassword { get; set; }
        public string Address { get; set; }

        public int TotalItemsInInvoice { get; set; }

    }
}
