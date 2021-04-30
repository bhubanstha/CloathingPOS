using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Bill
    {
        [Key]
        public Int64 BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public decimal VAT { get; set; }

        public virtual List<Sales> Sales { get; set; }
    }
}