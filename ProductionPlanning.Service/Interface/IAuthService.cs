using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Core.Model;
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
        Task<ApplicationUser> GetValidateRefreshToken(string refreshToken);
        Task<TokenResponseDTO> RefreshToken(ApplicationUser user);
    }
}
