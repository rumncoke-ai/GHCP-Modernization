using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LegacyShop.Data.Entities
{
    public class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }

        [Required, StringLength(75)]
        public string FirstName { get; set; }

        [Required, StringLength(75)]
        public string LastName { get; set; }

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
