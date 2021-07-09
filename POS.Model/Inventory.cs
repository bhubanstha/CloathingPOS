using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Inventory : Product
    {

        [Required(ErrorMessage = "Purchase rate per piece is required.")]
        [Range(1, (double)Int64.MaxValue, ErrorMessage = "Purchase rate should be greater than 0")]
        public decimal PurchaseRate { get; set; }



        [Required(ErrorMessage = "Retail rate per piece is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Retail rate should be greater than 0")]
        public decimal RetailRate { get; set; }


        [Required(ErrorMessage = "Item purchase quantity is required.")]
        [Range(0, (double)(int.MaxValue), ErrorMessage = "Item purchase quantity should be greater than 0")]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "Purchase Date is required.")]
        [DataType(DataType.Date)]
        [Column(TypeName = "DATE")]
        public DateTime FirstPurchaseDate { get; set; }

        public bool IsDeleted { get; set; }


        [ForeignKey("Branch")]
        public Int64? BranchId { get; set; }

        [ForeignKey("User")]
        public Int64? UserId { get; set; }

        
        public virtual List<InventoryHistory> InventoryHistories { get; set; }


        public virtual User User { get; set; }
        public virtual Branch Branch { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Size);
        }
    }
}
