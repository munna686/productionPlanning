﻿using Microsoft.AspNetCore.Identity;
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

        public async Task<ServiceResponse> inActiveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ResponseUtility.SendFailResponce("User does not exist");
            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            return ResponseUtility.SendSuccessResponce(user);
        }
    }
}
