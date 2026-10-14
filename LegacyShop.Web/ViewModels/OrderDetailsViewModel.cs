using System;
using System.Collections.Generic;

namespace LegacyShop.Web.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel()
        {
            Items = new List<OrderLineViewModel>();
        }

        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public IList<OrderLineViewModel> Items { get; set; }
    }

    public class OrderLineViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
