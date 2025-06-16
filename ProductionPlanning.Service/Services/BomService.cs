using Microsoft.EntityFrameworkCore;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Core.Interfaces;
using ProductionPlanning.Core.Model;
using ProductionPlanning.Service.Interface;
using ProductionPlanning.Utility;
using System.Reflection.Metadata;

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

                // Add to bomDetails list
                bomDetails.Add(new BomDetail
                {
                    MaterialId = detail.MaterialId,
                    Quantity = detail.Quantity,
                    UOM = detail.UOM
                });
            }

            var bomMaster = new BomMaster
            {
                BomCode = await unitOfWork.Bom.GenerateNextBomCodeAsync(),
                ProductId = dto.ProductId, // Corrected this line
                OutputQuantiy = dto.OutputQuantity,
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
            if (bom == null) return ResponseUtility.SendFailResponce("BOM Not Found");

            //Update BOM Master
            bom.ProductId = dto.ProductId;
            bom.OutputQuantiy = dto.OutputQuantity;
            bom.UpdatedBy = currentUser.FullName;
            bom.UpdatedOn = DateTime.Now;

            //Existing Detail
            var existingDetail = bom.BomDetails.Select(b => b.BomDetailId).ToList();

            //incoming Detail
            var incomingDetail = dto.BomDetails.Where(d => d.BomDetailId.HasValue).Select(d => d.BomDetailId.Value).ToList();

            //remove deleted details
            var detailsToRemove = bom.BomDetails.Where(d => !incomingDetail.Contains(d.BomDetailId)).ToList();
            foreach (var bomDetail in detailsToRemove) bom.BomDetails.Remove(bomDetail);

            //update existing and add new
            foreach (var bomDetail in dto.BomDetails)
            {
                if (bomDetail.BomDetailId.HasValue)
                {
                    var exist = bom.BomDetails.FirstOrDefault(d => d.BomDetailId == bomDetail.BomDetailId);
                    if (exist is not null)
                    {
                        exist.MaterialId = bomDetail.MaterialId;
                        exist.Quantity = bomDetail.Quantity;
                        exist.UOM = bomDetail.UOM;
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
            var user = currentUserService.getCurrentUser();
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
        public async Task<ServiceResponse> startBomForProd(int bomId, string Remarks)
        {
            var transaction = unitOfWork.BeginTransactionAsync();
            try
            {
                var user = currentUserService.getCurrentUser();
                var bom = await unitOfWork.Bom.GetAllQueryable().Where(a => a.BomId == bomId).Include(d => d.BomDetails).FirstOrDefaultAsync();
                if (bom == null) return ResponseUtility.SendFailResponce("Bom Not found");
                if (!bom.IsApproved) return ResponseUtility.SendFailResponce("Bom is not approved");


                foreach (var detail in bom.BomDetails)
                {

                    RawMaterial mat = await unitOfWork.Material.GetById(detail.MaterialId);
                    if (mat == null) return ResponseUtility.SendFailResponce($"Material not found for ID {detail.MaterialId}");
                    if (detail.Quantity > mat.StockQty) return ResponseUtility.SendFailResponce($"Insufficient quantity for {mat.MaterialName}");
                    mat.StockQty -= detail.Quantity;
                    await unitOfWork.save();
                }


                BomLog bomLog = new BomLog
                {
                    BomId = bom.BomId,
                    StartedBy = user.FullName,
                    StartingTime = DateTime.Now,
                    isFinished = false,
                    isActive = true,
                    Remarks = Remarks
                };
                bom.TotalProcessed += 1;
                await unitOfWork.BomLog.Add(bomLog);
                await unitOfWork.save();
                await unitOfWork.CommitAsync();
                return ResponseUtility.SendSuccessResponce(bomLog);
            }
            catch (Exception)
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<ServiceResponse> FinishBom(int bomLogId)
        {
            var bomlog = await unitOfWork.BomLog.GetById(bomLogId);
            var bom = await unitOfWork.Bom.GetById(bomlog.BomId);
            var product = await unitOfWork.Product.GetById(bom.ProductId);
            if (bomlog.isFinished) return ResponseUtility.SendFailResponce("Bom is finished");

            product.StockQty += bom.OutputQuantiy;
            bomlog.isFinished = true;
            bomlog.EndingTime = DateTime.Now;
            await unitOfWork.save();
            return ResponseUtility.SendSuccessResponce(bomlog);
        }
        public async Task<ServiceResponse> CancelBom(int BomLogId)
        {
            var trans = await unitOfWork.BeginTransactionAsync();
            try
            {
                var user = currentUserService.getCurrentUser();
                var bomlog = await unitOfWork.BomLog.GetById(BomLogId);
                var bom = await unitOfWork.Bom.GetAllQueryable().Where(a => a.BomId == bomlog.BomId).Include(d => d.BomDetails).FirstOrDefaultAsync();
                if (bomlog.isFinished) return ResponseUtility.SendFailResponce("Can't cancel finished bom");
                //rollback bomdetails product
                foreach (var detail in bom.BomDetails)
                {

                    RawMaterial mat = await unitOfWork.Material.GetById(detail.MaterialId);
                    if (mat == null) return ResponseUtility.SendFailResponce($"Material not found for ID {detail.MaterialId}");
                    if (detail.Quantity > mat.StockQty) return ResponseUtility.SendFailResponce($"Insufficient quantity for {mat.MaterialName}");
                    mat.StockQty += detail.Quantity;
                    await unitOfWork.save();
                }
                bomlog.CancelBy = user.FullName;
                bomlog.CaneledOn = DateTime.Now;
                await unitOfWork.save();
                await unitOfWork.CommitAsync();
                return ResponseUtility.SendSuccessResponce(bomlog);
            }
            catch (Exception)
            {
                await unitOfWork.RollbackAsync();
                throw;
            }


        }
        public async Task<ServiceResponse> getAllRunningBoms()
        {
            var data = await unitOfWork.BomLog.GetAllQueryable().Where(b => b.isFinished == false && b.isActive == true).ToListAsync();
            return ResponseUtility.SendGetAllResponce(data);
        }

    }
}
