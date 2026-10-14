using System.Collections.Generic;
using LegacyShop.Data.Entities;

namespace LegacyShop.Data.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Product> GetActiveProducts(int? categoryId);
        Product GetProductWithCategory(int id);
    }
}
