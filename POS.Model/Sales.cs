using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Sales 
    {
        public int Id { get; set; }
        public int SalesQuantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }

        [ForeignKey("Inventory")]
        public int ProductId { get; set; }

        [ForeignKey("Bill")]
        public Int64 BillNo { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Bill Bill { get; set; }


    }
}