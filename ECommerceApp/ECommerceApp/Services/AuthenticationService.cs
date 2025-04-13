using ECommerceApp.Helpers;
using ECommerceApp.Models;
using ECommerceApp.Repositories;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ECommerceApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICartRepository _cartRepository;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICartRepository cartRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartRepository = cartRepository;
        }

        public async Task<OperationResult> LoginAsync(LoginUserViewModel model)
        {
            var appUser = await _userManager.FindByNameAsync(model.UserName);

            if (appUser == null)
            {
                return OperationResult.Failure("Invalid UserName or Password");
            }

            bool found = await _userManager.CheckPasswordAsync(appUser, model.Password);
            if (!found)
            {
                return OperationResult.Failure("Invalid UserName or Password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim("ProfilePicture", appUser.ImageUrl ?? "/images/users/default-user.png")
            };

            await _signInManager.SignInWithClaimsAsync(appUser, model.RememberMe, claims);

            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");
            return OperationResult.Success(isAdmin);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public AuthenticationProperties ConfigureGoogleAuthProperties(string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        }

        public async Task<OperationResult> HandleGoogleResponseAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return OperationResult.Failure("Could not retrieve external login information");
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                var existingUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (existingUser != null)
                {
                    var isAdminRole = await _userManager.IsInRoleAsync(existingUser, "Admin");
                    return OperationResult.Success(isAdminRole);
                }
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email;

            if (string.IsNullOrEmpty(email))
            {
                return OperationResult.Failure("Email is required for external authentication");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    FullName = name,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return OperationResult.Failure(createResult.Errors.Select(e => e.Description).ToList());
                }
                Cart cart = new Cart { UserId = user.Id };
                await _cartRepository.CreateAsync(cart);
                await _cartRepository.SaveAsync();
            }
            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                return OperationResult.Failure(addLoginResult.Errors.Select(e => e.Description).ToList());
            }
            await _signInManager.SignInAsync(user, isPersistent: true);

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            return OperationResult.Success(isAdmin);
        }
    }
}
