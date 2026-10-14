using System;
using System.Collections.Generic;
using LegacyShop.Data.Entities;

namespace LegacyShop.Business.Interfaces
{
    public interface ICategoryService : IDisposable
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}
