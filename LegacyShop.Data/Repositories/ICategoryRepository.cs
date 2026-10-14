using LegacyShop.Data.Entities;

namespace LegacyShop.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Category GetCategoryWithProducts(int id);
    }
}
