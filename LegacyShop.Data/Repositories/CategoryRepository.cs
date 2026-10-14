using System.Data.Entity;
using System.Linq;
using LegacyShop.Data.Entities;

namespace LegacyShop.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(StoreDbContext context)
            : base(context)
        {
        }

        public Category GetCategoryWithProducts(int id)
        {
            return Context.Categories
                .Include(c => c.Products)
                .SingleOrDefault(c => c.CategoryId == id);
        }
    }
}
