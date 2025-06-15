using ProductionPlanning.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Service.Interface
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Login(LoginDTO dto);
        Task<ServiceResponse> Logout();
    }
}
