using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    public class Customer : EntityBase
    {
        [Required(ErrorMessage = "Customer name is required.")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(50, ErrorMessage = "Customer name can be 50 characters long")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Customer address is required.")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(250, ErrorMessage = "Customer address can be 250 characters long")]
        public string Address { get; set; }


        [Column(TypeName = "VARCHAR")]
        [MaxLength(300, ErrorMessage = "Customer address can be 300 characters long")]
        public string GoogleMap { get; set; }

        [Required(ErrorMessage = "Customer mobile-1 is required.")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(10, ErrorMessage = "Customer mobile-1 can be 10 characters long")]
        public string Mobile1 { get; set; }


        [Column(TypeName = "VARCHAR")]
        [MaxLength(10, ErrorMessage = "Customer mobile-2 can be 10 characters long")]
        public string Mobile2 { get; set; }


        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Address);
        }
    }
}
