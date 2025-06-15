using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Core.Model;
using ProductionPlanning.Service.Interface;
using System.Security.Claims;
using System.Text.Json;

namespace ProductionPlanning.Service.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public CurrentUserService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<CurrentUserDto?> GetCurrentUserAsync()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) return null;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return new CurrentUserDto
            {
                Id = userId,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.FirstOrDefault(),
            };
        }
        public CurrentUserDto getCurrentUser()
        {
            var UserJson = _contextAccessor.HttpContext?.Request.Cookies["CurrentUser"];
            if(!string.IsNullOrEmpty(UserJson))
            {
                var currentUser = JsonSerializer.Deserialize<CurrentUserDto>(UserJson);
                return currentUser;
            }
            return null;
        }
    }
}
