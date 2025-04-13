using ECommerceApp.DTOs;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace ECommerceApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ImageService _imageService;
        public UserService(UserManager<User> userManager, ImageService imageService)
        {
            _userManager = userManager;
            _imageService = imageService;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsers(string searchTerm = "", string currentAdminId = "")
        {
            var query = _userManager.Users.Where(u => u.Id != currentAdminId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u =>
                    u.FullName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.UserName.Contains(searchTerm));
            }

            return await query.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                UserName = u.UserName
            }).ToListAsync();
        }
        public async Task DeleteUser(User user)
        {
            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                _imageService.DeleteImage(user.ImageUrl);
            }
            await _userManager.DeleteAsync(user);
        }
        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                ImageUrl = user.ImageUrl,
                ImageFile = null
            };
        }

        public async Task UpdateUserProfileAsync(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user == null) return;

            user.FullName = userDto.FullName;
            user.Address = userDto.Address;

            if (userDto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    _imageService.DeleteImage(user.ImageUrl);
                }

                user.ImageUrl = await _imageService.SaveImageAsync(userDto.ImageFile, "users");
            }

            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateUserPasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Password update failed.");
            }
        }
        public async Task<bool> HasPasswordAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.HasPasswordAsync(user);
        }
        public async Task<bool> SetUserPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }
            if (await _userManager.HasPasswordAsync(user))
            {
                throw new ApplicationException("User already has a password set");
            }
            var result = await _userManager.AddPasswordAsync(user, newPassword);
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to set password: {errors}");
            }

            return true;
        }
    }
}
