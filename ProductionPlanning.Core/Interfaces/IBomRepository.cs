using ProductionPlanning.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Core.Interfaces
{
    public interface IBomRepository : IGenericRepository<BomMaster>
    {
        Task<string> GenerateNextBomCodeAsync();
    }
}
