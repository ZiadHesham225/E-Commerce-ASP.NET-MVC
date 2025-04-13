using ECommerceApp.Models;
using ECommerceApp.Repositories;
namespace ECommerceApp.Services
{
    public class CartService : ICartService
    {
        public readonly ICartRepository _cartRepository;
        public readonly IProductRepository _productRepository;
        public readonly ICartItemRepository _cartItemRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, ICartItemRepository cartItemsRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _cartItemRepository = cartItemsRepository;
        }

        public async Task<bool> AddToCartAsync(string userId, int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.Stock < quantity)
            {
                return false;
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, ItemCount = quantity };
                await _cartRepository.CreateAsync(cart);
                await _cartRepository.SaveAsync();
            }

            var cartItem = await _cartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem != null)
            {
                cart.ItemCount += quantity;
                cartItem.Quantity += quantity;
                _cartItemRepository.Update(cartItem);
                _cartRepository.Update(cart);
                await _cartItemRepository.SaveAsync();
                await _cartRepository.SaveAsync();
            }
            else
            {
                cart.ItemCount += quantity;
                _cartRepository.Update(cart);
                await _cartItemRepository.CreateAsync(new CartItem { CartId = cart.Id, ProductId = productId, Quantity = quantity });
                await _cartItemRepository.SaveAsync();
                await _cartRepository.SaveAsync();
            }

            return true;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                cart.ItemCount = 0;
                _cartRepository.Update(cart);
                await _cartRepository.SaveAsync();
            }
            await _cartRepository.ClearCartByUserIdAsync(userId);
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return cart?.CartItems ?? new List<CartItem>();
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return cart?.ItemCount ?? 0;
        }

        public async Task RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return;

            var cartItem = await _cartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem != null)
            {
                cart.ItemCount -= cartItem.Quantity;
                if (cart.ItemCount < 0) cart.ItemCount = 0;

                _cartRepository.Update(cart);
                await _cartItemRepository.DeleteAsync(cartItem.Id);
                await _cartItemRepository.SaveAsync();
                await _cartRepository.SaveAsync();
            }
        }

        public async Task<bool> UpdateCartItemQuantityAsync(string userId, int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.Stock < quantity)
            {
                return false;
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            var cartItem = await _cartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem != null)
            {
                int difference = quantity - cartItem.Quantity;
                cart.ItemCount += difference;
                if (cart.ItemCount < 0) cart.ItemCount = 0;

                cartItem.Quantity = quantity;
                _cartItemRepository.Update(cartItem);
                _cartRepository.Update(cart);
                await _cartItemRepository.SaveAsync();
                await _cartRepository.SaveAsync();
            }

            return true;
        }

        public async Task<int> GetProductQuantityInCart(string userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return 0;
            return await _cartItemRepository.GetProductQuantityAsync(cart.Id, productId);
        }
    }
}