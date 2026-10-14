using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyShop.Data.Entities
{
    public class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            IsActive = true;
        }

        public int ProductId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        [Range(0, 999999)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
