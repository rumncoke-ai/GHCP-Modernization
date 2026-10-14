using System.Web.Mvc;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data.Entities;
using LegacyShop.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LegacyShop.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {
        [TestMethod]
        public void Details_ReturnsNotFoundForInactiveProduct()
        {
            var products = new Mock<IProductService>();
            products.Setup(s => s.GetProduct(5))
                .Returns(new Product { ProductId = 5, Name = "Hidden", IsActive = false });
            var categories = new Mock<ICategoryService>();
            var controller = new ProductsController(products.Object, categories.Object);

            ActionResult result = controller.Details(5);

            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
    }
}
