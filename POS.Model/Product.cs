using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Product : EntityBase
    {
        public string Name { get; set; }

        [MaxLength(5)]
        public string Size { get; set; }

        [MaxLength(10)]
        public string Color { get; set; }

        [MaxLength(30)]
        public string ColorName { get; set; }

        [ForeignKey("Category")]
        public Int64 CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}