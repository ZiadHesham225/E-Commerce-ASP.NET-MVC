using ECommerceApp.Models;

namespace ECommerceApp.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
    }
}
