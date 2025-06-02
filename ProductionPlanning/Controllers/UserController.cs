using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Service.Interface;
using ProductionPlanning.Utility;

namespace ProductionPlanning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService userService;
        public UserController(IUserService userService) => this.userService = userService;
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
            => this.CustomApiResponse(await userService.Login(dto));
        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
            => this.CustomApiResponse(await userService.Logout());
        [HttpPost]
        public async Task<IActionResult> CreateUser(AddUserDTO dto)
            => this.CustomApiResponse(await userService.CreateUser(dto));
    }
}
