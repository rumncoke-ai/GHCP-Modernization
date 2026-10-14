using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyShop.Data.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal LineTotal
        {
            get { return Quantity * UnitPrice; }
        }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
