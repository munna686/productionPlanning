using Microsoft.AspNetCore.Identity;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        public UserService(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            this.unitOfWork = unitOfWork;
            this._signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<ServiceResponse> CreateUser(AddUserDTO dto)
        {
            var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                if (dto == null) return ResponseUtility.SendFailResponce("Invalid Data");
                var user = new ApplicationUser
                {
                    FullName = dto.FirstName + " " + dto.LastName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    UserName = dto.Email, // UserName must be set, Identity needs this
                };

                var result = await _userManager.CreateAsync(user, dto.PasswordHash);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ResponseUtility.SendFailResponce(result.Errors.First().Description);
                }
                await transaction.CommitAsync();
                return ResponseUtility.SendSuccessResponce("User Created Successfully");
            }
            catch (Exception ex) { await transaction.RollbackAsync(); return ResponseUtility.SendFailResponce($"Error: {ex.Message}"); }

        }

        public async Task<AuthResponseDTO> Login(LoginDTO dto)
        {
            var user = await unitOfWork.User.FindByEmailAsync(dto.Email);
            if (user == null) return ResponseUtility.SendLoginFail(dto);
            var success = await unitOfWork.User.CheckPasswordAsync(user,dto.Password);
            if(!success) return ResponseUtility.SendLoginFail(dto);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim (ClaimTypes.Role,user.Role)
            };
            var accessToken = _jwtService.GenerateAccessToken(claims);
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

        public async Task<ServiceResponse> Logout()
        {
            await _signInManager.SignOutAsync();
            return ResponseUtility.SendLogOutSuccess(true);
        }

        public async Task<ServiceResponse> getAllUser()
        {
            var data = await unitOfWork.User.GetAll();
            return ResponseUtility.SendGetAllResponce(data);
        }

        public async Task<ServiceResponse> UpdateUser(UpdateUserDTO user)
        {
            if (user == null) return ResponseUtility.SendFailResponce("Invalid Data");
            var exist = await unitOfWork.User.FindById(user.Id);
            if(exist is null) return ResponseUtility.SendFailResponce("Invalid Data");
            exist.FullName = user.FirstName + " " + user.LastName;
            exist.PhoneNumber = user.PhoneNumber;
            exist.Email = user.Email;
            unitOfWork.User.Update(exist);
            await unitOfWork.save();
            return ResponseUtility.SendUpdateResponce(exist);
        }
    }
}
