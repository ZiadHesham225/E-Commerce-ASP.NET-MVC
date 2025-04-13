namespace ECommerceApp.Services
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSession(string userId);

        Task<bool> VerifyPaymentStatus(string sessionId);
    }
}