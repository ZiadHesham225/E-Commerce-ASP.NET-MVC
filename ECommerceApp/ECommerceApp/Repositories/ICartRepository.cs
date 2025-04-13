using ECommerceApp.Models;

namespace ECommerceApp.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task ClearCartByUserIdAsync(string userId);
    }
}
