using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface IWishListService
    {
        Task<bool> AddToWishlistAsync(string userId, int productId);
        Task RemoveFromWishlistAsync(string userId, int productId);
        Task<IEnumerable<Wishlist>> GetWishlistItemsAsync(string userId);
        Task ClearWishlistAsync(string userId);
    }
}
