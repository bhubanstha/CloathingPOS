using System;

namespace POS.Model
{
    public class InventoryHistory : EntityBase
    {
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}