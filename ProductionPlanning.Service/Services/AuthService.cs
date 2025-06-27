using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Core.Interfaces;
using ProductionPlanning.Core.Model;
using ProductionPlanning.Service.Interface;
using ProductionPlanning.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        public AuthService(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            this.unitOfWork = unitOfWork;
            this._signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }
        public async Task<AuthResponseDTO> Login(LoginDTO dto)
        {
            var user = await unitOfWork.User.FindByEmailAsync(dto.Email);
            if (user == null) return ResponseUtility.SendLoginFail(dto);
            var success = await unitOfWork.User.CheckPasswordAsync(user, dto.Password);
            if (!success) return ResponseUtility.SendLoginFail(dto);
            
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            var response = new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserName = user.UserName,
                CurrentUser = new CurrentUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role
                }

            };
            return response;
        }

        public async Task<ApplicationUser> GetValidateRefreshToken(string refreshToken)
        {
            var data = await unitOfWork.User.GetAllQueryable().Where(r => r.RefreshToken == refreshToken && r.RefreshTokenExpiryTime > DateTime.Now).FirstOrDefaultAsync();
            return data;
        }

        public async Task<TokenResponseDTO> RefreshToken(ApplicationUser user)
        {
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            return new TokenResponseDTO
            {
                NewAccessToken = accessToken,
                NewRefreshToken = refreshToken
            };
        } 

        public async Task<ServiceResponse> Logout()
        {
            await _signInManager.SignOutAsync();
            return ResponseUtility.SendLogOutSuccess(true);
        }
    }
}
