using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LegacyShop.Business.Interfaces;

namespace LegacyShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Index()
        {
            return View(_productService.GetProducts(null, false).Take(4));
        }

    }
}
