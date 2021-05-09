﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    public class Inventory : Product
    {
        public decimal PurchaseRate { get; set; }
        public decimal RetailRate { get; set; }
        public int Quantity { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime FirstPurchaseDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual List<InventoryHistory> InventoryHistories { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Size);
        }
    }
}