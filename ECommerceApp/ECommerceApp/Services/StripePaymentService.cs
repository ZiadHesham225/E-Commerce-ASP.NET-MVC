using ECommerceApp.Models;
using ECommerceApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Stripe;
using Stripe.Checkout;

namespace ECommerceApp.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;

        public StripePaymentService(
            UserManager<User> userManager,
            IConfiguration configuration,
            ICartService cartService,
            IOrderService orderService,
            ITransactionRepository transactionRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _cartService = cartService;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<string> CreateCheckoutSession(string userId)
        {
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            var user = await _userManager.FindByIdAsync(userId);
            string userEmail = user.Email;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = userEmail,
                SuccessUrl = _configuration["Stripe:SuccessUrl"] + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = _configuration["Stripe:CancelUrl"],
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", userId }
                }
            };

            foreach (var item in cartItems)
            {
                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                            Description = item.Product.Description
                        }
                    },
                    Quantity = item.Quantity
                };

                options.LineItems.Add(lineItem);
            }

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }

        public async Task<bool> VerifyPaymentStatus(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            return session.PaymentStatus == "paid";
        }
    }
}