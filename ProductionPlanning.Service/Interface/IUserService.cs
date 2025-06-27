using ProductionPlanning.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Service.Interface
{
    public interface IUserService
    {
        
        Task<ServiceResponse> getAllUser();
        Task<ServiceResponse> CreateUser(AddUserDTO dto);
        Task<ServiceResponse> UpdateUser(UpdateUserDTO user);
        Task<ServiceResponse> inActiveUser(string userId);
    }
}
