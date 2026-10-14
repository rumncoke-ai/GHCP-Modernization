using System.Linq;
using System.Web.Mvc;
using LegacyShop.Business.Interfaces;
using LegacyShop.Business.Services;

namespace LegacyShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController()
            : this(new ProductService())
        {
        }

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Index()
        {
            return View(_productService.GetProducts(null, false).Take(4));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
