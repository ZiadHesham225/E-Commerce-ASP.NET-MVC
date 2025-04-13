using ECommerceApp.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Services
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetRolesAsync();
        Task<bool> AddRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<List<string>> GetUnassignedRolesAsync(string userId);
        Task<List<string>> GetAssignedRolesAsync(string userId);
        Task<bool> AssignRolesAsync(string userId, List<string> roleNames);
        Task<bool> RemoveRolesAsync(string userId, List<string> roleNames);
        Task<List<UserDto>> GetUsersByRoleAsync(string roleName);
    }
}
