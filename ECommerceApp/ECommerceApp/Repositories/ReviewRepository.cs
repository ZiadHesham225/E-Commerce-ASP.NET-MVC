using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }

        public async Task<(IEnumerable<ReviewDto> Reviews, int TotalCount)> GetReviewsByProductAsync(int productId, int pageNumber, int pageSize)
        {
            var query = dbSet
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    ProductId = r.ProductId,
                    Rating = r.Rating,
                    UserId = r.UserId,
                    UserFullName = r.User.FullName
                });

            int totalCount = await query.CountAsync();

            var reviews = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (reviews, totalCount);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId)
        {
            return await dbSet.Where(r => r.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<Review>> GetUserReviewsForProductAsync(string userId, int productId)
        {
            return await dbSet.Where(r => r.UserId == userId && r.ProductId == productId).ToListAsync();
        }
    }
}
