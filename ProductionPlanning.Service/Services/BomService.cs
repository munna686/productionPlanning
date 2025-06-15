using Microsoft.EntityFrameworkCore;
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
    public class BomService : IBomService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        
        public BomService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<ServiceResponse> CreateBomAsync(BomMasterDTO dto)
        {
            var currentUser = currentUserService.getCurrentUser();
            var bomDetails = new List<BomDetail>();

            foreach (var detail in dto.BomDetails)
            {
                var mat = await unitOfWork.Material.GetById(detail.MaterialId);
                if (mat == null) return ResponseUtility.SendFailResponce($"Material not found for ID {detail.MaterialId}");
                if (detail.Quantity > mat.StockQty) return ResponseUtility.SendFailResponce($"Insufficient quantity for {mat.MaterialName}");

                // Add to bomDetails list
                bomDetails.Add(new BomDetail
                {
                    MaterialId = detail.MaterialId,
                    Quantity = detail.Quantity,
                    UOM = detail.UOM,
                    Remarks = detail.Remarks
                });
            }

            var bomMaster = new BomMaster
            {
                BomCode = await unitOfWork.Bom.GenerateNextBomCodeAsync(),
                ProductId = dto.ProductId, // Corrected this line
                CreatedBy = currentUser.FullName,
                CreatedOn = DateTime.Now,
                IsApproved = false,
                BomDetails = bomDetails
            };
            await this.unitOfWork.Bom.Add(bomMaster);
            await unitOfWork.save();
            return ResponseUtility.SendSuccessResponce(bomMaster);
        }

        public async Task<ServiceResponse> EditBom(EditBomMasterDTO dto)
        {
            var currentUser = currentUserService.getCurrentUser();
            var bom = await unitOfWork.Bom.GetById(dto.BomId);
            if(bom == null) return ResponseUtility.SendFailResponce("BOM Not Found");

            //Update BOM Master
            bom.ProductId = dto.ProductId;
            bom.UpdatedBy = currentUser.FullName;
            bom.UpdatedOn = DateTime.Now;

            //Existing Detail
            var existingDetail = bom.BomDetails.Select(b => b.BomDetailId).ToList();

            //incoming Detail
            var incomingDetail = dto.BomDetails.Where(d => d.BomDetailId.HasValue).Select(d => d.BomDetailId.Value).ToList();

            //remove deleted details
            var detailsToRemove = bom.BomDetails.Where(d => !incomingDetail.Contains(d.BomDetailId)).ToList();
            foreach(var bomDetail in detailsToRemove) bom.BomDetails.Remove(bomDetail);
            
            //update existing and add new
            foreach(var bomDetail in dto.BomDetails)
            {
                if (bomDetail.BomDetailId.HasValue)
                {
                    var exist = bom.BomDetails.FirstOrDefault(d => d.BomDetailId == bomDetail.BomDetailId);
                    if (exist is not null)
                    {
                        exist.MaterialId = bomDetail.MaterialId;
                        exist.Quantity = bomDetail.Quantity;
                        exist.UOM = bomDetail.UOM;
                        exist.Remarks = bomDetail.Remarks;
                    }
                }
                else
                {
                    //new item
                    bom.BomDetails.Add(new BomDetail
                    {
                        MaterialId = bomDetail.MaterialId,
                        Quantity = bomDetail.Quantity,
                        UOM = bomDetail.UOM,
                        Remarks = bomDetail.Remarks
                    });
                }
            }
            await unitOfWork.save();
            return ResponseUtility.SendUpdateResponce(bom);
        }

        public async Task<ServiceResponse> DeleteBomAsync(int bomId)
        {
            var bom = await unitOfWork.Bom.GetById(bomId);

            if (bom == null)
                return ResponseUtility.SendFailResponce("BOM not found");

            unitOfWork.Bom.Delete(bom);
            await unitOfWork.save();

            return ResponseUtility.SendHardDeleteResponce("BOM deleted successfully");
        }

        public async Task<ServiceResponse> GetByBomId(int id)
        {
            var bom = await unitOfWork.Bom.GetAllQueryable().Where(a => a.BomId == id).Include(d => d.BomDetails).FirstOrDefaultAsync();
            return ResponseUtility.SendGetAllResponce(bom);
        }

        public async Task<ServiceResponse> GetAllBom()
        {
            var data = await unitOfWork.Bom.GetAllQueryable().Include(d => d.BomDetails).ToListAsync();
            
            
            return ResponseUtility.SendGetAllResponce(data);
        }

        public async Task<ServiceResponse> ApproveBom(int bomId)
        {
            var user =  currentUserService.getCurrentUser();
            var bom = await unitOfWork.Bom.GetById(bomId);
            bom.ApprovedBy = user.FullName;
            bom.IsApproved = true;
            await unitOfWork.save();
            return ResponseUtility.SendSuccessResponce(bom);
        }

        public async Task<ServiceResponse> getEligibleBomsForProd()
        {
            var boms = await unitOfWork.Bom.GetAllQueryable().Where(b => b.IsApproved == true).ToListAsync();
            return ResponseUtility.SendGetAllResponce(boms);
        }
        public async Task<ServiceResponse> startBomForProd(int bomId)
        {
            var user = currentUserService.getCurrentUser();
            var bom = await unitOfWork.Bom.GetById(bomId);
            bom.TotalProcessed += 1;

            BomLog bomLog = new BomLog
            {
                BomId = bom.BomId,
                StartedBy = user.FullName,
                StartingTime = DateTime.Now,
                isFinished = false,
                isActive = true
            };
            unitOfWork

        }

    }
}
