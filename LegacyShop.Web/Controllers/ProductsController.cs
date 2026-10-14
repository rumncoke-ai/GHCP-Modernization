using Microsoft.AspNetCore.Mvc;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data.Entities;
using LegacyShop.Web.ViewModels;

namespace LegacyShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public ActionResult Index(int? categoryId)
        {
            var model = new ProductListViewModel
            {
                Products = _productService.GetProducts(categoryId, false),
                Categories = _categoryService.GetCategories(),
                SelectedCategoryId = categoryId
            };

            return View(model);
        }

        public ActionResult Details(int id)
        {
            Product product = _productService.GetProduct(id);
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
