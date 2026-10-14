using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LegacyShop.Data.Entities
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
