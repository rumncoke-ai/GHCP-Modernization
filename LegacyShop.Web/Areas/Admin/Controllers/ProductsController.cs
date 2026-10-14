using System;
using System.Linq;
using System.Web.Mvc;
using LegacyShop.Business.Interfaces;
using LegacyShop.Business.Services;
using LegacyShop.Data.Entities;
using LegacyShop.Web.ViewModels;

namespace LegacyShop.Web.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController()
            : this(new ProductService(), new CategoryService())
        {
        }

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return View(_productService.GetProducts(null, true));
        }

        public ActionResult Details(int id)
        {
            Product product = _productService.GetProduct(id);
            return product == null ? (ActionResult)HttpNotFound() : View(product);
        }

        public ActionResult Create()
        {
            PopulateCategories();
            return View(new ProductEditViewModel { IsActive = true });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productService.CreateProduct(MapProduct(model));
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            PopulateCategories(model.CategoryId);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Product product = _productService.GetProduct(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            PopulateCategories(product.CategoryId);
            return View(new ProductEditViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productService.UpdateProduct(MapProduct(model));
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            PopulateCategories(model.CategoryId);
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Product product = _productService.GetProduct(id);
            return product == null ? (ActionResult)HttpNotFound() : View(product);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                Product product = _productService.GetProduct(id);
                if (product == null)
                {
                    return HttpNotFound();
                }

                ModelState.AddModelError("", ex.Message);
                return View("Delete", product);
            }
        }

        private void PopulateCategories(int? selectedId = null)
        {
            ViewBag.CategoryId = new SelectList(
                _categoryService.GetCategories(),
                "CategoryId",
                "Name",
                selectedId);
        }

        private static Product MapProduct(ProductEditViewModel model)
        {
            return new Product
            {
                ProductId = model.ProductId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                IsActive = model.IsActive,
                CategoryId = model.CategoryId
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productService.Dispose();
                _categoryService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
