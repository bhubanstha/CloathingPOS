using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Sales : EntityBase
    {
        public int SalesQuantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }

        [ForeignKey("Inventory")]
        public Int64 ProductId { get; set; }

        [ForeignKey("Bill")]
        public Int64 BillNo { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Bill Bill { get; set; }


    }
}