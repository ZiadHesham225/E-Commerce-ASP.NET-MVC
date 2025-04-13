using ECommerceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, int rating, string? comment)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            bool done = await _reviewService.AddReviewAsync(userId, productId, rating, comment);
            if (!done)
            {
                return BadRequest();
            }
            return Ok();
        }

        public async Task<IActionResult> GetReviews(int id, int page = 1)
        {
            var reviews = await _reviewService.GetReviewsByProductAsync(id, page, 5);
            var totalReviews = reviews.TotalCount;
            var totalPages = (int)Math.Ceiling(totalReviews / 5.0);
            ViewBag.totalPages = totalPages;
            ViewBag.currentPage = page;
            return Json(new
            {
                products = reviews.Reviews,
                pagination = new
                {
                    currentPage = page,
                    totalPages = totalPages,
                    totalProducts = totalReviews
                }
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateReview(int reveiwId, int rating, string? comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            bool done = await _reviewService.UpdateReviewAsync(userId, reveiwId, rating, comment);
            if (!done)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            bool done = await _reviewService.DeleteReviewAsync(userId, reviewId);
            if (!done)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> GetUserReviewsForProduct(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var reviews = await _reviewService.GetUserReviewsForProductAsync(userId, productId);
            return Json(reviews);
        }
    }
}