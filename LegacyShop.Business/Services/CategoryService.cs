using System;
using System.Collections.Generic;
using System.Linq;
using LegacyShop.Business.Interfaces;
using LegacyShop.Data;
using LegacyShop.Data.Entities;
using LegacyShop.Data.Repositories;

namespace LegacyShop.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService()
            : this(new UnitOfWork(new StoreDbContext()))
        {
        }

        public CategoryService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _unitOfWork.Categories.GetAll().OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int id)
        {
            return _unitOfWork.Categories.GetCategoryWithProducts(id);
        }

        public void CreateCategory(Category category)
        {
            Validate(category);
            _unitOfWork.Categories.Add(category);
            _unitOfWork.Complete();
        }

        public void UpdateCategory(Category category)
        {
            Validate(category);
            Category existing = _unitOfWork.Categories.GetById(category.CategoryId);
            if (existing == null)
            {
                throw new InvalidOperationException("The category no longer exists.");
            }

            existing.Name = category.Name;
            existing.Description = category.Description;
            _unitOfWork.Complete();
        }

        public void DeleteCategory(int id)
        {
            Category category = _unitOfWork.Categories.GetCategoryWithProducts(id);
            if (category == null)
            {
                throw new InvalidOperationException("The category no longer exists.");
            }

            if (category.Products.Any())
            {
                throw new InvalidOperationException("A category containing products cannot be deleted.");
            }

            _unitOfWork.Categories.Remove(category);
            _unitOfWork.Complete();
        }

        private static void Validate(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new InvalidOperationException("A category name is required.");
            }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
