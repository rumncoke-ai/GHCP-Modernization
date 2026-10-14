using System;
using System.Web.Mvc;
using LegacyShop.Business.Interfaces;
using LegacyShop.Business.Services;
using LegacyShop.Data.Entities;

namespace LegacyShop.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController()
            : this(new CategoryService())
        {
        }

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return View(_categoryService.GetCategories());
        }

        public ActionResult Details(int id)
        {
            Category category = _categoryService.GetCategory(id);
            return category == null ? (ActionResult)HttpNotFound() : View(category);
        }

        public ActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.CreateCategory(category);
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(category);
        }

        public ActionResult Edit(int id)
        {
            Category category = _categoryService.GetCategory(id);
            return category == null ? (ActionResult)HttpNotFound() : View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.UpdateCategory(category);
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(category);
        }

        public ActionResult Delete(int id)
        {
            Category category = _categoryService.GetCategory(id);
            return category == null ? (ActionResult)HttpNotFound() : View(category);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _categoryService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                Category category = _categoryService.GetCategory(id);
                if (category == null)
                {
                    return HttpNotFound();
                }

                ModelState.AddModelError("", ex.Message);
                return View("Delete", category);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _categoryService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
