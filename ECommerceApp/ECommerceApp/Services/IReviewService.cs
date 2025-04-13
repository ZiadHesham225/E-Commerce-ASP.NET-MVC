using ECommerceApp.DTOs;
using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(string userId, int productId, int rating, string? comment);
        Task<bool> DeleteReviewAsync(string userId, int reviewId);
        Task<(IEnumerable<ReviewDto> Reviews, int TotalCount)> GetReviewsByProductAsync(int productId, int pageNumber, int pageSize);
        Task<IEnumerable<Review>> GetUserReviewsForProductAsync(string userId, int productId);
        Task<bool> UpdateReviewAsync(string userId, int reviewId, int rating, string? comment);

    }
}
