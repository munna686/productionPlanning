using Microsoft.EntityFrameworkCore;
using ProductionPlanning.Core.Interfaces;
using ProductionPlanning.Core.Model;
using ProductionPlanning.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Infrastructure.Repos
{
    public class BomRepository : GenericRepository<BomMaster>, IBomRepository
    {
        public BomRepository(InventoryDataContext context) : base(context) { }
        public async Task<string> GenerateNextBomCodeAsync()
        {
            var lastBom = await context.BomMasters
                .OrderByDescending(b => b.BomId)
                .FirstOrDefaultAsync();

            string nextBomCode = "BOM-0001";

            if (lastBom != null && !string.IsNullOrEmpty(lastBom.BomCode))
            {
                string lastCode = lastBom.BomCode.Replace("BOM-", "");
                if (int.TryParse(lastCode, out int lastNumber))
                {
                    nextBomCode = "BOM-" + (lastNumber + 1).ToString("D4");
                }
            }

            return nextBomCode;
        }


    }

}
