using System;
using System.Collections.Generic;
using LegacyShop.Data.Entities;

namespace LegacyShop.Business.Interfaces
{
    public interface IProductService : IDisposable
    {
        IEnumerable<Product> GetProducts(int? categoryId, bool includeInactive);
        Product GetProduct(int id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
