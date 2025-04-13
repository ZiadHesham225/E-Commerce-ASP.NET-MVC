using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Repositories
{
    public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Wishlist>> GetWishlistByUserIdAsync(string userId)
        {
            return await dbSet.Where(w => w.UserId == userId).ToListAsync();
        }
        public async Task<Wishlist> GetWishlistItemAsync(string userId, int productId)
        {
            return await dbSet.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        }
        public async Task ClearWishlistByUserIdAsync(string userId)
        {
            var wishlistItems = await GetWishlistByUserIdAsync(userId);
            if (wishlistItems != null)
            {
                dbSet.RemoveRange(wishlistItems);
                await SaveAsync();
            }
        }
    }
}
