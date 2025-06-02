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
    public class RawMaterialRepository : GenericRepository<RawMaterial>, IRawMaterialRepository
    {
        public RawMaterialRepository(InventoryDataContext context) : base(context) { }
    }
}
