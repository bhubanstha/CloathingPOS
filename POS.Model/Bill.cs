using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
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

        [ForeignKey("Branch")]
        public Int64? BranchId { get; set; }

        [ForeignKey("User")]
        public Int64? UserId { get; set; }

        public virtual List<Sales> Sales { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual User User { get; set; }
    }
}