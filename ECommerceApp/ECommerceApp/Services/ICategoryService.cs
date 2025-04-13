using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category categoryDto);
        Task UpdateCategoryAsync(Category categoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
