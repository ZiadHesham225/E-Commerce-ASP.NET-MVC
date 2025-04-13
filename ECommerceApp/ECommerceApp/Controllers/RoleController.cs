using ECommerceApp.Models;
using ECommerceApp.Services;
using ECommerceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        public RoleController(IRoleService roleService, UserManager<User> userManager, IUserService userService)
        {
            _roleService = roleService;
            _userManager = userManager;
            _userService = userService;
        }
        public async Task<IActionResult> Index(string searchTerm)
        {
            string currentAdminId = _userManager.GetUserId(User);
            var users = await _userService.GetAllUsers(searchTerm, currentAdminId);
            return View(users);
        }
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var unassignedRoles = await _roleService.GetUnassignedRolesAsync(userId);
            var assignedRoles = await _roleService.GetAssignedRolesAsync(userId);
            var userRoleVM = new UserRoleViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                UnassignedRoles = unassignedRoles,
                AssignedRoles = assignedRoles
            };

            return View(userRoleVM);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, List<string> roles)
        {
            if (roles?.Any() == true)
            {
                await _roleService.AssignRolesAsync(userId, roles);
            }
            return RedirectToAction("AssignRole", new { userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRoles(string userId, List<string> roles)
        {
            if (roles?.Any() == true)
            {
                await _roleService.RemoveRolesAsync(userId, roles);
            }
            return RedirectToAction("AssignRole", new { userId });
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                await _roleService.AddRoleAsync(roleName);
            }
            return RedirectToAction("ManageRoles");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                await _roleService.DeleteRoleAsync(roleName);
            }
            return RedirectToAction("ManageRoles");
        }
        public async Task<IActionResult> ManageRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            return View(roles);
        }
    }
}

