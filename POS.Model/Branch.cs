using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Branch : EntityBase
    {

        [Required(ErrorMessage = "Branch name is required.")]
        [MaxLength(50, ErrorMessage = "Branch name can be upto 50 character long")]
        [Column(TypeName = "VARCHAR")]
        public string BranchName { get; set; }


        [Required(ErrorMessage = "Branch address is required.")]
        [MaxLength(300, ErrorMessage = "Branch address can be upto 300 character long")]
        [Column(TypeName = "VARCHAR")]
        public string BranchAddress { get; set; }


        [Required(ErrorMessage = "Company info is required.")]
        [ForeignKey("Shop")]
        public Int64 ShopId { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
