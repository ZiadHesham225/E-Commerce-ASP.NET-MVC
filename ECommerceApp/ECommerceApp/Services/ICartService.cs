using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(string userId, int productId, int quantity);
        Task RemoveFromCartAsync(string userId, int productId);
        Task<bool> UpdateCartItemQuantityAsync(string userId, int productId, int quantity);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);
        Task ClearCartAsync(string userId);
        Task<int> GetProductQuantityInCart(string userId, int productId);
        Task<int> GetCartItemCountAsync(string userId);
    }
}
