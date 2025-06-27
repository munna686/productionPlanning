using Microsoft.AspNetCore.Mvc;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Infrastructure.Repos;
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
        private readonly IJwtService jwtService;
        public AuthController(IAuthService authService,IJwtService jwtService)
        {
            this.authService = authService;
            this.jwtService = jwtService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var response = await this.authService.Login(dto);
            if (response == null) return Unauthorized(dto);


            var user = response.CurrentUser;
            var userJson = JsonSerializer.Serialize(user);
            Response.Cookies.Append("CurrentUser", userJson, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(7)
            });

            Response.Cookies.Append("RefreshToken", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(7)
            });

            return Ok(response);
        }

        [HttpPut("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if(string.IsNullOrEmpty(refreshToken)) return Unauthorized("Invalid refreshToken");
            var User = await authService.GetValidateRefreshToken(refreshToken);
            if(User == null) return Unauthorized("Invalid or expired RefreshToken");

            var tokens = await authService.RefreshToken(User);
            Response.Cookies.Append("RefreshToken", tokens.NewRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddDays(7)
            });
            return Ok(new { AccessToken = tokens.NewAccessToken });
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
