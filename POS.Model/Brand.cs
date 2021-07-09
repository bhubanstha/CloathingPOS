using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Brand : EntityBase
    {

        [Required(ErrorMessage = "Brand name is required.")]
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }
    }
}