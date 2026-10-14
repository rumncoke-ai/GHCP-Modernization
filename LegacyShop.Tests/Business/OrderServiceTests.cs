using System;
using System.Collections.Generic;
using System.Linq;
using LegacyShop.Business.Services;
using LegacyShop.Data.Entities;
using LegacyShop.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LegacyShop.Tests.Business
{
    [TestClass]
    public class OrderServiceTests
    {
        [TestMethod]
        public void CreateOrder_CalculatesTotalAndReducesStock()
        {
            var product = new Product
            {
                ProductId = 4,
                Name = "Keyboard",
                Price = 20m,
                StockQuantity = 10,
                IsActive = true
            };
            Order savedOrder = null;
            var products = new Mock<IProductRepository>();
            products.Setup(r => r.GetById(4)).Returns(product);
            var customers = new Mock<ICustomerRepository>();
            customers.Setup(r => r.GetById(2)).Returns(new Customer { CustomerId = 2 });
            var orders = new Mock<IOrderRepository>();
            orders.Setup(r => r.Add(It.IsAny<Order>())).Callback<Order>(o =>
            {
                o.OrderId = 99;
                savedOrder = o;
            });
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(u => u.Products).Returns(products.Object);
            unitOfWork.SetupGet(u => u.Customers).Returns(customers.Object);
            unitOfWork.SetupGet(u => u.Orders).Returns(orders.Object);
            var service = new OrderService(unitOfWork.Object);

            Order result = service.CreateOrder(2, new Dictionary<int, int> { { 4, 3 } });

            Assert.AreSame(savedOrder, result);
            Assert.AreEqual(60m, result.TotalAmount);
            Assert.AreEqual(7, product.StockQuantity);
            Assert.AreEqual(3, result.OrderItems.Single().Quantity);
            unitOfWork.Verify(u => u.Complete(), Times.Once);
        }

        [TestMethod]
        public void CreateOrder_RejectsQuantityGreaterThanStock()
        {
            var product = new Product
            {
                ProductId = 4,
                Name = "Keyboard",
                Price = 20m,
                StockQuantity = 2,
                IsActive = true
            };
            var products = new Mock<IProductRepository>();
            products.Setup(r => r.GetById(4)).Returns(product);
            var customers = new Mock<ICustomerRepository>();
            customers.Setup(r => r.GetById(2)).Returns(new Customer { CustomerId = 2 });
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(u => u.Products).Returns(products.Object);
            unitOfWork.SetupGet(u => u.Customers).Returns(customers.Object);
            var service = new OrderService(unitOfWork.Object);

            InvalidOperationException exception = Assert.ThrowsExactly<InvalidOperationException>(
                () => service.CreateOrder(2, new Dictionary<int, int> { { 4, 3 } }));

            StringAssert.Contains(exception.Message, "not enough stock");
            Assert.AreEqual(2, product.StockQuantity);
            unitOfWork.Verify(u => u.Complete(), Times.Never);
        }
    }
}
