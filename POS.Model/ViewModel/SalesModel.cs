using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model.ViewModel
{
    public class SalesModel
    {
        public Int64 ProductId { get; set; }
        public Int64 CategoryId { get; set; }
        public Int64 BrandId { get; set; }
        public Int64 BranchId { get; set; }

        public int SalesQuantity { get; set; }
        public decimal Discount { get; set; }
        
        public decimal RetailRate { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }        
        public string ColorName { get; set; }        
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
    }

    public class BillModel
    {
        public Int64 BillNo { get; set; }
        public DateTime BillDate { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        public string BillTo { get; set; }
        public string BillingAddress { get; set; }
        public string BillingPAN { get; set; }
        public decimal VAT{ get; set; }
        public decimal GrandTotal { get; set; }
    }
}
