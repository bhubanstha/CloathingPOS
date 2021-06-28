using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core.Model
{
    public class Shop : EntityBase
    {

        [Required(ErrorMessage = "Branch name is required.")]
        [MaxLength(50)]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Shop Name is required.")]
        [MaxLength(200)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Shop Address is required.")]
        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string PANNumber { get; set; }

        [MaxLength(200)]
        public string LogoPath { get; set; }
        public bool CalculateVATOnSales { get; set; }
        public bool PrintInvoice { get; set; }

        [MaxLength(10)]
        public string PdfPassword { get; set; }
    }
}
