using ECommerceApp.DTOs;
using ECommerceApp.Models;

namespace ECommerceApp.Repositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId);
        Task<IEnumerable<Review>> GetUserReviewsForProductAsync(string userId, int productId);
        Task<(IEnumerable<ReviewDto> Reviews, int TotalCount)> GetReviewsByProductAsync(int productId, int pageNumber, int pageSize);
    }
}
