using ECommerceApp.Models;
using ECommerceApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartRepository;

        public CartCountViewComponent(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int count = 0;
            if (User.Identity.IsAuthenticated)
            {
                var userId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                count = cart?.ItemCount ?? 0;
            }

            return View(count);
        }
    }
}
