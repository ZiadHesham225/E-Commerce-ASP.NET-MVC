using ECommerceApp.Models;

namespace ECommerceApp.Repositories
{
    public interface IWishlistRepository : IGenericRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetWishlistByUserIdAsync(string userId);
        Task<Wishlist> GetWishlistItemAsync(string userId, int productId);
        Task ClearWishlistByUserIdAsync(string userId);
    }
}
