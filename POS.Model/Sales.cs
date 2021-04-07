using System;

namespace POS.Model
{
    public class Sales : Product
    {
        public int SalesQuantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal VAT { get; set; }
    }
}