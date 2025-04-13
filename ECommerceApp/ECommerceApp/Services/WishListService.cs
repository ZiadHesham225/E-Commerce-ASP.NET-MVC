using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services
{
    public class WishListService : IWishListService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;

        public WishListService(IWishlistRepository wishlistRepository, IProductRepository productRepository)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> AddToWishlistAsync(string userId, int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            var wishlistItem = await _wishlistRepository.GetWishlistItemAsync(userId, productId);
            if (wishlistItem == null)
            {
                await _wishlistRepository.CreateAsync(new Wishlist { UserId = userId, ProductId = productId });
            }
            return true;
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistItemsAsync(string userId)
        {
          return await _wishlistRepository.GetWishlistByUserIdAsync(userId);
        }

        public async Task RemoveFromWishlistAsync(string userId, int productId)
        {
            var wishlistItem = await _wishlistRepository.GetWishlistItemAsync(userId, productId);
            if (wishlistItem != null)
            {
                await _wishlistRepository.DeleteAsync(wishlistItem.Id);
            }
        }
        public async Task ClearWishlistAsync(string userId)
        {
            await _wishlistRepository.ClearWishlistByUserIdAsync(userId);
        }
    }
}
