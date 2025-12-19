using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repositories
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<CartItem> GetCartItemAsync(int cartId, int productId)
        {
            return await dbSet.Include(ci => ci.Product).FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
        public async Task<int> GetProductQuantityAsync(int cartId, int productId)
        {
            var cartItem = await GetCartItemAsync(cartId, productId);
            return cartItem?.Quantity ?? 0;
        }

        public async Task<Dictionary<int, int>> GetProductQuantitiesAsync(int cartId, IEnumerable<int> productIds)
        {
            return await dbSet
                .Where(ci => ci.CartId == cartId && productIds.Contains(ci.ProductId))
                .ToDictionaryAsync(ci => ci.ProductId, ci => ci.Quantity);
        }
    }
}
