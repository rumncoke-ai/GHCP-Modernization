using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LegacyShop.Web.ViewModels
{
    public class OrderCreateViewModel
    {
        public OrderCreateViewModel()
        {
            Customers = new List<SelectListItem>();
            Products = new List<OrderProductSelectionViewModel>();
        }

        [Display(Name = "Customer")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a customer.")]
        public int CustomerId { get; set; }

        public IList<SelectListItem> Customers { get; set; }
        public IList<OrderProductSelectionViewModel> Products { get; set; }
    }

    public class OrderProductSelectionViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
        public bool Selected { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
