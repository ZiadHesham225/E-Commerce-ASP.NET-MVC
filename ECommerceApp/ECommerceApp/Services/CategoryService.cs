using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.ToList();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return category;
        }

        public async Task AddCategoryAsync(Category category)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(category.Name);
            if (existingCategory != null)
            {
                throw new InvalidOperationException("Category name must be unique.");
            }

            var newCategory = new Category { Name = category.Name };
            await _categoryRepository.CreateAsync(newCategory);
            await _categoryRepository.SaveAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(category.Name);
            if (existingCategory != null && existingCategory.Id != category.Id)
            {
                throw new InvalidOperationException("Category name must be unique.");
            }

            var newCategory = await _categoryRepository.GetByIdAsync(category.Id);
            if (category == null) return;

            category.Name = newCategory.Name;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return;

            await _categoryRepository.DeleteAsync(category.Id);
            await _categoryRepository.SaveAsync();
        }
    }
}
