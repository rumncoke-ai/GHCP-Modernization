using System;
using System.Collections.Generic;
using System.Linq;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data;
using LegacyShop.Data.Entities;
using LegacyShop.Data.Repositories;

namespace LegacyShop.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService()
            : this(new UnitOfWork(new StoreDbContext()))
        {
        }

        public ProductService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> GetProducts(int? categoryId, bool includeInactive)
        {
            if (!includeInactive)
            {
                return _unitOfWork.Products.GetActiveProducts(categoryId);
            }

            IEnumerable<Product> products = _unitOfWork.Products.GetAll();
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            return products.OrderBy(p => p.Name).ToList();
        }

        public Product GetProduct(int id)
        {
            return _unitOfWork.Products.GetProductWithCategory(id);
        }

        public void CreateProduct(Product product)
        {
            ValidateProduct(product);
            _unitOfWork.Products.Add(product);
            _unitOfWork.Complete();
        }

        public void UpdateProduct(Product product)
        {
            ValidateProduct(product);
            Product existing = _unitOfWork.Products.GetById(product.ProductId);
            if (existing == null)
            {
                throw new InvalidOperationException("The product no longer exists.");
            }

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.StockQuantity = product.StockQuantity;
            existing.IsActive = product.IsActive;
            existing.CategoryId = product.CategoryId;
            _unitOfWork.Complete();
        }

        public void DeleteProduct(int id)
        {
            Product product = _unitOfWork.Products.GetById(id);
            if (product == null)
            {
                throw new InvalidOperationException("The product no longer exists.");
            }

            if (product.OrderItems.Any())
            {
                throw new InvalidOperationException("Products used by an order cannot be deleted. Mark the product inactive instead.");
            }

            _unitOfWork.Products.Remove(product);
            _unitOfWork.Complete();
        }

        private static void ValidateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new InvalidOperationException("A product name is required.");
            }

            if (product.Price < 0)
            {
                throw new InvalidOperationException("A product price cannot be negative.");
            }

            if (product.StockQuantity < 0)
            {
                throw new InvalidOperationException("Stock quantity cannot be negative.");
            }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
