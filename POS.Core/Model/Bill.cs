using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POS.Core.Model
{
    public class Bill : EntityBase
    {
        public DateTime BillDate { get; set; }
        public decimal VAT { get; set; }

        [MaxLength(100)]
        public string BillTo { get; set; }

        [MaxLength(200)]
        public string BillingAddress { get; set; }

        [MaxLength(20)]
        public string BillingPAN { get; set; }

        public virtual List<Sales> Sales { get; set; }
    }
}