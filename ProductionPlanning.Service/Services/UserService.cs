using Microsoft.AspNetCore.Identity;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Core.Interfaces;
using ProductionPlanning.Core.Model;
using ProductionPlanning.Service.Interface;
using ProductionPlanning.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this._signInManager = signInManager;
            _userManager = userManager;
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

        public async Task<ServiceResponse> Login(LoginDTO dto)
        {
            var user = await unitOfWork.User.FindByEmailAsync(dto.Email);
            if (user == null) return ResponseUtility.SendLoginFail(dto);
            var success = await unitOfWork.User.CheckPasswordAsync(user, dto.Password);
            if (!success) return ResponseUtility.SendLoginFail(dto);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return ResponseUtility.SendLoginSuccess(dto);
        }

        public async Task<ServiceResponse> Logout()
        {
            await _signInManager.SignOutAsync();
            return ResponseUtility.SendLogOutSuccess(true);
        }
    }
}
