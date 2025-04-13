using ECommerceApp.DTOs;
using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers(string searchTerm = "", string currentAdminId = "");
        Task DeleteUser(User user);
        Task<UserDto> GetUserByIdAsync(string id);
        Task UpdateUserProfileAsync(UserDto userDto);
        Task UpdateUserPasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> HasPasswordAsync(string userId);
        Task<bool> SetUserPasswordAsync(string userId, string newPassword);
    }
}
