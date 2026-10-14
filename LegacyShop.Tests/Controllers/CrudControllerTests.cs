using LegacyShop.Business.Interfaces;
using LegacyShop.Data.Entities;
using LegacyShop.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LegacyShop.Tests.Controllers
{
    [TestClass]
    public class CrudControllerTests
    {
        [TestMethod]
        public void CategoriesCreate_ValidModelCreatesAndRedirectsToIndex()
        {
            var categories = new Mock<ICategoryService>();
            var controller = new CategoriesController(categories.Object);
            var category = new Category { Name = "New category" };

            ActionResult result = controller.Create(category);

            categories.Verify(service => service.CreateCategory(category), Times.Once);
            var redirect = Assert.IsInstanceOfType<RedirectToActionResult>(result);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void CustomersEdit_ValidModelUpdatesAndRedirectsToIndex()
        {
            var customers = new Mock<ICustomerService>();
            var controller = new CustomersController(customers.Object);
            var customer = new Customer
            {
                CustomerId = 7,
                FirstName = "Alex",
                LastName = "Johnson",
                Email = "alex@example.test"
            };

            ActionResult result = controller.Edit(customer);

            customers.Verify(service => service.UpdateCustomer(customer), Times.Once);
            var redirect = Assert.IsInstanceOfType<RedirectToActionResult>(result);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void CategoriesDelete_PostDeletesAndRedirectsToIndex()
        {
            var categories = new Mock<ICategoryService>();
            var controller = new CategoriesController(categories.Object);

            ActionResult result = controller.DeleteConfirmed(12);

            categories.Verify(service => service.DeleteCategory(12), Times.Once);
            var redirect = Assert.IsInstanceOfType<RedirectToActionResult>(result);
            Assert.AreEqual("Index", redirect.ActionName);
        }
    }
}
