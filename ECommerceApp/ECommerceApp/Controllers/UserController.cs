using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, IUserService userService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _userService = userService;
            _signInManager = signInManager;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var users = await _userService.GetAllUsers(searchTerm);
            return View(users);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Admins cannot delete their profiles.";
                return RedirectToAction("Profile");
            }
            await _userService.DeleteUser(user);
            return RedirectToAction("SignOut","Account");
        }
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();
            ViewData["HasPassword"] = await _userService.HasPasswordAsync(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserDto userDto)
        {
            if (!ModelState.IsValid) return View("Profile", userDto);

            await _userService.UpdateUserProfileAsync(userDto);
            TempData["SuccessMessage"] = "Profile updated successfully!";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var existingClaim = await _userManager.GetClaimsAsync(user);
            var profilePictureClaim = existingClaim.FirstOrDefault(c => c.Type == "ProfilePicture");

            if (profilePictureClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, profilePictureClaim);
            }
            await _userManager.AddClaimAsync(user, new Claim("ProfilePicture", user.ImageUrl ?? "/images/users/default-user.png"));
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string currentPassword, string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                TempData["ErrorMessage"] = "Please provide both current and new passwords.";
                return RedirectToAction("Profile");
            }

            try
            {
                await _userService.UpdateUserPasswordAsync(userId, currentPassword, newPassword);
                TempData["SuccessMessage"] = "Password updated successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Failed to update password.";
            }

            return RedirectToAction("Profile");
        }
        [HttpPost]
        public async Task<IActionResult> SetPassword(string newPassword, string confirmPassword) // For External Users
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                TempData["ErrorMessage"] = "Please provide both password fields.";
                return RedirectToAction("Profile");
            }

            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match.";
                return RedirectToAction("Profile");
            }

            try
            {
                await _userService.SetUserPasswordAsync(userId, newPassword);
                TempData["SuccessMessage"] = "Password set successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to set password: " + ex.Message;
            }

            return RedirectToAction("Profile");
        }
    }
}
