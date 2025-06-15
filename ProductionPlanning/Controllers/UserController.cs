using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Core.Model;
using ProductionPlanning.Service.Interface;
using ProductionPlanning.Utility;
using System.Text.Json;

namespace ProductionPlanning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var response = await this.userService.Login(dto);
            if(response == null) return Unauthorized(dto);
            

            var user = response.CurrentUser;
            var userJson = JsonSerializer.Serialize(user);
            Response.Cookies.Append("CurrentUser", userJson, new CookieOptions
            {
                HttpOnly = false, 
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            Response.Cookies.Append("RefreshToken",response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(7)
            });

            return Ok(response);
        }
        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
            => this.CustomApiResponse(await userService.Logout());
        [HttpPost]
        public async Task<IActionResult> CreateUser(AddUserDTO dto)
            => this.CustomApiResponse(await userService.CreateUser(dto));
        
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
            => this.CustomApiResponse(await userService.getAllUser());
        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO dto) 
            => this.CustomApiResponse(await userService.UpdateUser(dto));

    }
}
