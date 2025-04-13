using ECommerceApp.Models;

namespace ECommerceApp.Repositories
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task<int> GetProductQuantityAsync(int cartId, int productId);
    }
}
