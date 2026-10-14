using System;
using LegacyShop.Business.Services;
using LegacyShop.Data.Entities;
using LegacyShop.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LegacyShop.Tests.Business
{
    [TestClass]
    public class ProductServiceTests
    {
        [TestMethod]
        public void CreateProduct_RejectsNegativePrice()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var service = new ProductService(unitOfWork.Object);
            var product = new Product { Name = "Invalid", Price = -1m, StockQuantity = 1 };

            InvalidOperationException exception =
                Assert.ThrowsException<InvalidOperationException>(() => service.CreateProduct(product));

            Assert.AreEqual("A product price cannot be negative.", exception.Message);
            unitOfWork.Verify(u => u.Complete(), Times.Never);
        }

        [TestMethod]
        public void UpdateProduct_CopiesEditableValuesAndSaves()
        {
            var existing = new Product
            {
                ProductId = 7,
                Name = "Old",
                Price = 10m,
                StockQuantity = 2,
                CategoryId = 1
            };
            var products = new Mock<IProductRepository>();
            products.Setup(r => r.GetById(7)).Returns(existing);
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(u => u.Products).Returns(products.Object);
            var service = new ProductService(unitOfWork.Object);

            service.UpdateProduct(new Product
            {
                ProductId = 7,
                Name = "Updated",
                Description = "Changed",
                Price = 25m,
                StockQuantity = 9,
                CategoryId = 3,
                IsActive = false
            });

            Assert.AreEqual("Updated", existing.Name);
            Assert.AreEqual(25m, existing.Price);
            Assert.AreEqual(9, existing.StockQuantity);
            Assert.AreEqual(3, existing.CategoryId);
            Assert.IsFalse(existing.IsActive);
            unitOfWork.Verify(u => u.Complete(), Times.Once);
        }
    }
}
