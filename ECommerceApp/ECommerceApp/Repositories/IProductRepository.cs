using ECommerceApp.DTOs;
using ECommerceApp.Models;

namespace ECommerceApp.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int departmentId);
        Task<Product> GetProductWithCategoryAndReviewsByIdAsync(int id);
        Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        IQueryable<ProductDto> GetProductsWithCategories();
        Task<Product> GetProducWithCategoryByIdAsync(int id);
    }
}
