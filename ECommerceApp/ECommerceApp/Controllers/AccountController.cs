using ECommerceApp.Services;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAccountService accountService, IAuthenticationService authenticationService)
        {
            _accountService = accountService;
            _authenticationService = authenticationService;
        }

        #region Local Authentication
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.RegisterUserAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Product");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authenticationService.LoginAsync(model);

            if (result.Succeeded)
            {
                return result.IsAdmin
                    ? RedirectToAction("Dashboard", "Admin")
                    : RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError("", "Invalid UserName or Password");
            return View(model);
        }
        #endregion

        #region Google Authentication
        [HttpGet]
        public IActionResult GoogleLogin(string returnUrl)
        {
            var properties = _authenticationService.ConfigureGoogleAuthProperties(
                Url.Action(nameof(GoogleResponse), "Account", new { returnUrl }));

            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await _authenticationService.HandleGoogleResponseAsync();

            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            return result.IsAdmin
                ? RedirectToAction("Dashboard", "Admin")
                : RedirectToAction("Index", "Product");
        }
        #endregion

        #region Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.SendPasswordResetEmailAsync(
                model.Email,
                Url.Action("ResetPassword", "Account", null, Request.Scheme));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Invalid password reset token");
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.ResetPasswordAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion
    }
}