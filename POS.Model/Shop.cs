using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    public class Shop : EntityBase
    {

        [Required(ErrorMessage = "Shop Name is required.")]
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "VARCHAR")]
        public string PANNumber { get; set; }

        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string LogoPath { get; set; }
        public bool CalculateVATOnSales { get; set; }
        public bool PrintInvoice { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "VARCHAR")]
        public string PdfPassword { get; set; }
    }
}
