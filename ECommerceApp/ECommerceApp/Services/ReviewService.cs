using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;

        public ReviewService(IReviewRepository reviewRepository, IProductRepository productRepository)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
        }
        public async Task<bool> AddReviewAsync(string userId, int productId, int rating, string? comment)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            var review = new Review
            {
                UserId = userId,
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.CreateAsync(review);
            await _reviewRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteReviewAsync(string userId, int reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null || review.UserId != userId)
            {
                return false;
            }

            await _reviewRepository.DeleteAsync(reviewId);
            await _reviewRepository.SaveAsync();
            return true;
        }

        public async Task<(IEnumerable<ReviewDto> Reviews, int TotalCount)> GetReviewsByProductAsync(int productId, int pageNumber, int pageSize)
        {
            return await _reviewRepository.GetReviewsByProductAsync(productId, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Review>> GetUserReviewsForProductAsync(string userId, int productId)
        {
            return await _reviewRepository.GetUserReviewsForProductAsync(userId, productId);
        }

        public async Task<bool> UpdateReviewAsync(string userId, int reviewId, int rating, string? comment)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null || review.UserId != userId)
            {
                return false;
            }
            review.Rating = rating;
            review.Comment = comment;

            _reviewRepository.Update(review);
            await _reviewRepository.SaveAsync();
            return true;
        }
    }
}
