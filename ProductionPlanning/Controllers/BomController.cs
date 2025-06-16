using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductionPlanning.Core.DTOs;
using ProductionPlanning.Service.Interface;
using ProductionPlanning.Utility;

namespace ProductionPlanning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BomController : ControllerBase
    {
        private readonly IBomService service;
        public BomController(IBomService service)
            => this.service = service;
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateBom(BomMasterDTO bom)
            => this.CustomApiResponse(await service.CreateBomAsync(bom));
        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> UpdateBom(EditBomMasterDTO bom)
            => this.CustomApiResponse(await service.EditBom(bom));
        [HttpGet("getall")]
        [Authorize]
        public async Task<IActionResult> GetAll()
            => this.CustomApiResponse(await service.GetAllBom());
        [HttpGet("getById")]
        [Authorize]
        public async Task<IActionResult> getById(int id)
            => this.CustomApiResponse(await service.GetByBomId(id));
        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteById(int id)
            => this.CustomApiResponse(await service.DeleteBomAsync(id));
        [HttpPut("approve")]
        [Authorize]
        public async Task<IActionResult> ApprovedBom(int bomId)
            => this.CustomApiResponse(await service.ApproveBom(bomId));
        [HttpPost("start/bom")]
        [Authorize]
        public async Task<IActionResult> StartBom(int bomId,string Remarks)
            => this.CustomApiResponse(await service.startBomForProd(bomId,Remarks));
        [HttpPut("finish/bom")]
        [Authorize]
        public async Task<IActionResult> FinishBom(int bomId)
            => this.CustomApiResponse(await service.FinishBom(bomId));

        [HttpPut("cancel/bom")]
        [Authorize]
        public async Task<IActionResult> cancel(int bomlogid)
            => this.CustomApiResponse(await service.CancelBom(bomlogid));

        [HttpGet("running")]
        [Authorize]
        public async Task<IActionResult> getAllRunningTask()
            => this.CustomApiResponse(await service.getAllRunningBoms());
        

    }
}
