using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyShop.Data.Entities
{
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            OrderDate = DateTime.Now;
            Status = "Pending";
        }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required, StringLength(30)]
        public string Status { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
