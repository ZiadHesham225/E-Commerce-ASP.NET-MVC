using ECommerceApp.Helpers;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Authentication;

namespace ECommerceApp.Services
{
    public interface IAuthenticationService
    {
        Task<OperationResult> LoginAsync(LoginUserViewModel model);
        Task SignOutAsync();
        AuthenticationProperties ConfigureGoogleAuthProperties(string redirectUrl);
        Task<OperationResult> HandleGoogleResponseAsync();
    }
}
