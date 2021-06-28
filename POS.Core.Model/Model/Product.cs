using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Core.Model
{
    public class Product : EntityBase
    {

        [Required(ErrorMessage = "Item Name is required.")]
        [MaxLength(50, ErrorMessage = "Item name can be upto 50 character long.")]
        public string Name { get; set; }



        [Required(ErrorMessage = "Item size is required.")]
        [MaxLength(5, ErrorMessage = "Size should be upto 5 character long.")]
        public string Size { get; set; }




        [Required(ErrorMessage = "Item colour is required.")]
        [MaxLength(9, ErrorMessage = "Color value should be upto 9 character long")]
        public string Color { get; set; }



        [Required(ErrorMessage = "Color name is required.")]
        [MaxLength(20, ErrorMessage = "Color name can be upto 20 characters long only.")]
        public string ColorName { get; set; }




        [ForeignKey("Category")]
        [Required(ErrorMessage = "Category is required")]
        [Range(1, (double)Int64.MaxValue, ErrorMessage = "Category should be a valid value.")]
        public Int64 CategoryId { get; set; }


        [ForeignKey("Brand")]
        [Required(ErrorMessage = "Brand is required")]
        [Range(1, (double)Int64.MaxValue, ErrorMessage = "Category should be a valid value.")]
        public Int64 BrandId { get; set; }



        public virtual Category Category { get; set; }
        public virtual Brand Brand{ get; set; }
    }
}