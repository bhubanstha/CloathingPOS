using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Category : EntityBase
    {
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }
    }
}