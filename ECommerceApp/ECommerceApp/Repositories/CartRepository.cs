using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            return await dbSet.Include(c => c.CartItems)
                              .ThenInclude(ci => ci.Product)
                              .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task ClearCartByUserIdAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                cart.CartItems.Clear();
                await SaveAsync();
            }
        }
    }
}
