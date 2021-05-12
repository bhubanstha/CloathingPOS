using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Model
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }

        [ForeignKey("Category")]
        public Int64 CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}