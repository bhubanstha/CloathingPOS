using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class InventoryHistory : EntityBase
    {
        public int Quantity { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal RetailRate { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime PurchaseDate { get; set; }

        [ForeignKey("Inventory")]
        public Int64 InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}