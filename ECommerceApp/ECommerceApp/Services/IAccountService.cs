using ECommerceApp.Helpers;
using ECommerceApp.ViewModels;

namespace ECommerceApp.Services
{
    public interface IAccountService
    {
        Task<OperationResult> RegisterUserAsync(RegisterUserViewModel model);
        Task<OperationResult> SendPasswordResetEmailAsync(string email, string baseUrl);
        Task<OperationResult> ResetPasswordAsync(ResetPasswordViewModel model);
    }
}
