using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await dbSet.Where(p => p.CategoryId == categoryId).ToListAsync();
        }
        public IQueryable<ProductDto> GetProductsWithCategories()
        {
            return dbSet.Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryName = p.Category.Name
                });
        }
        public async Task<Product> GetProducWithCategoryByIdAsync(int id)
        {
            return await dbSet.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product> GetProductWithCategoryAndReviewsByIdAsync(int id)
        {
            return await dbSet.Include(p => p.Category).Include(p => p.Reviews).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            return await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}
