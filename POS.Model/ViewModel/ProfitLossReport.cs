using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model.ViewModel
{
    public class ProfitLossReport
    {
        public DateTime BillingDate { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string ItemName { get; set; }
        public string Color { get; set; }
        public string ItemCode { get; set; }
        public string Size { get; set; }
        public int SalesQty { get; set; }
        public decimal PurchaseTotal { get; set; }
        public decimal SalesTotal { get; set; }

        public decimal Profit { get; set; }
    }
}
