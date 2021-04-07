using System;

namespace POS.Model
{
    public class InventoryHistory
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}