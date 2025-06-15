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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var response = await this.authService.Login(dto);
            if(response == null) return Unauthorized(dto);
            

            var user = response.CurrentUser;
            var userJson = JsonSerializer.Serialize(user);
            Response.Cookies.Append("CurrentUser", userJson, new CookieOptions
            {
                HttpOnly = false, 
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(7)
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
        {
            Response.Cookies.Delete("CurrentUser");
            Response.Cookies.Delete("RefreshToken");

            return this.CustomApiResponse(await authService.Logout());

        }


    }
}
