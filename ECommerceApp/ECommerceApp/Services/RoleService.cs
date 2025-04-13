using ECommerceApp.DTOs;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null ? (await _userManager.GetRolesAsync(user)).ToList() : new List<string>();
        }

        public async Task<List<string>> GetUnassignedRolesAsync(string userId)
        {
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var assignedRoles = await GetUserRolesAsync(userId);
            return allRoles.Except(assignedRoles).ToList();
        }

        public async Task<List<string>> GetAssignedRolesAsync(string userId)
        {
            return await GetUserRolesAsync(userId);
        }

        public async Task<bool> AssignRolesAsync(string userId, List<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.AddToRolesAsync(user, roleNames);
            return result.Succeeded;
        }

        public async Task<bool> RemoveRolesAsync(string userId, List<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.RemoveFromRolesAsync(user, roleNames);
            return result.Succeeded;
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Select(u => new UserDto { Id = u.Id, UserName = u.UserName, Email = u.Email, FullName = u.FullName }).ToList();
        }
        public async Task<bool> AddRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return false;

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            if (roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                return false;
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }
}
