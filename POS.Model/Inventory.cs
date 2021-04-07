using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    public class Inventory : Product
    {
        public decimal PurchaseRate { get; set; }
        public int Quantity { get; set; }
        public DateTime FirstPurchaseDate { get; set; }

        public virtual List<InventoryHistory> InventoryHistories { get; set; }
    }
}
