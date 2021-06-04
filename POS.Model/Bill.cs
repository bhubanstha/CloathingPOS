using System;
using System.Collections.Generic;

namespace POS.Model
{
    public class Bill : EntityBase
    {
        public DateTime BillDate { get; set; }
        public decimal VAT { get; set; }
        public string BillTo { get; set; }

        public virtual List<Sales> Sales { get; set; }
    }
}