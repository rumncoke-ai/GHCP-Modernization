using System.Collections.Generic;
using LegacyShop.Data.Entities;

namespace LegacyShop.Web.ViewModels
{
    public class ProductListViewModel
    {
        public ProductListViewModel()
        {
            Products = new List<Product>();
            Categories = new List<Category>();
        }

        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}
