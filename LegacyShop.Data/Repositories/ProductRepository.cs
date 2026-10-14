using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LegacyShop.Data.Entities;

namespace LegacyShop.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(StoreDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Product> GetActiveProducts(int? categoryId)
        {
            IQueryable<Product> query = Context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return query.OrderBy(p => p.Name).ToList();
        }

        public Product GetProductWithCategory(int id)
        {
            return Context.Products
                .Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == id);
        }
    }
}
