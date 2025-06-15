using ProductionPlanning.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Service.Interface
{
    public interface IBomService
    {
        Task<ServiceResponse> CreateBomAsync(BomMasterDTO dto);
        Task<ServiceResponse> EditBom(EditBomMasterDTO dto);
        Task<ServiceResponse> DeleteBomAsync(int bomId);
        Task<ServiceResponse> GetByBomId(int id);
        Task<ServiceResponse> GetAllBom();
        Task<ServiceResponse> ApproveBom(int bomId);
        Task<ServiceResponse> getEligibleBomsForProd();
    }
}
