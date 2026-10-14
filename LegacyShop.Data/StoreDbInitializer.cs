using System;
using System.Collections.Generic;
using System.Data.Entity;
using LegacyShop.Data.Entities;

namespace LegacyShop.Data
{
    public class StoreDbInitializer : CreateDatabaseIfNotExists<StoreDbContext>
    {
        protected override void Seed(StoreDbContext context)
        {
            var electronics = new Category { Name = "Electronics", Description = "Useful electronic products." };
            var home = new Category { Name = "Home", Description = "Products for the home and office." };
            var books = new Category { Name = "Books", Description = "Technical and general-interest books." };

            var keyboard = new Product
            {
                Name = "Mechanical Keyboard",
                Description = "A sturdy wired keyboard with tactile switches.",
                Price = 79.95m,
                StockQuantity = 25,
                Category = electronics
            };
            var monitor = new Product
            {
                Name = "24-inch Monitor",
                Description = "A practical 1080p office monitor.",
                Price = 189.00m,
                StockQuantity = 12,
                Category = electronics
            };
            var lamp = new Product
            {
                Name = "Desk Lamp",
                Description = "Adjustable LED desk lamp.",
                Price = 34.50m,
                StockQuantity = 40,
                Category = home
            };
            var book = new Product
            {
                Name = "Legacy Systems Handbook",
                Description = "A fictional guide to maintaining established software.",
                Price = 49.99m,
                StockQuantity = 18,
                Category = books
            };

            var customer = new Customer
            {
                FirstName = "Alex",
                LastName = "Johnson",
                Email = "alex.johnson@example.test",
                Phone = "555-0100",
                Address = "100 Main Street",
                City = "Seattle",
                PostalCode = "98101"
            };

            context.Categories.AddRange(new[] { electronics, home, books });
            context.Products.AddRange(new[] { keyboard, monitor, lamp, book });
            context.Customers.Add(customer);
            context.SaveChanges();

            var order = new Order
            {
                CustomerId = customer.CustomerId,
                OrderDate = DateTime.Today.AddDays(-2),
                Status = "Completed",
                TotalAmount = keyboard.Price + (2 * lamp.Price),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = keyboard.ProductId, Quantity = 1, UnitPrice = keyboard.Price },
                    new OrderItem { ProductId = lamp.ProductId, Quantity = 2, UnitPrice = lamp.Price }
                }
            };

            context.Orders.Add(order);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
