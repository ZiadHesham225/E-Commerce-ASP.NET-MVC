using ECommerceApp.Helpers;
using ECommerceApp.Models;
using ECommerceApp.Repositories;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICartRepository _cartRepository;
        private readonly IEmailService _emailService;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICartRepository cartRepository,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartRepository = cartRepository;
            _emailService = emailService;
        }

        public async Task<OperationResult> RegisterUserAsync(RegisterUserViewModel model)
        {
            User user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                Address = model.Address
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return OperationResult.Failure(result.Errors.Select(e => e.Description).ToList());
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            string userId = user.Id;
            Cart cart = new Cart { UserId = userId };
            await _cartRepository.CreateAsync(cart);
            await _cartRepository.SaveAsync();

            return OperationResult.Success();
        }

        public async Task<OperationResult> SendPasswordResetEmailAsync(string email, string baseUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return OperationResult.Success();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{baseUrl}?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

            var subject = "Reset Your Password";
            var message = $@"
            <h1>Reset Your Password</h1>
            <p>Please reset your password by clicking the link below:</p>
            <p><a href='{resetLink}'>Reset Password</a></p>
            <p>If you did not request a password reset, please ignore this email.</p>
            ";

            try
            {
                await _emailService.SendEmailAsync(email, subject, message);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Error sending email. Please try again later.");
            }
        }

        public async Task<OperationResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return OperationResult.Success();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                return OperationResult.Failure(result.Errors.Select(e => e.Description).ToList());
            }

            return OperationResult.Success();
        }
    }
}
