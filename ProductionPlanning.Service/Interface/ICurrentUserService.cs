using ProductionPlanning.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Service.Interface
{
    public interface ICurrentUserService
    {
        Task<CurrentUserDto?> GetCurrentUserAsync();
    }
}
