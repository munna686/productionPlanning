using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Service.Interface;
using ProductionPlanning.Service.Services;
using ProductionPlanning.Utility;

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
