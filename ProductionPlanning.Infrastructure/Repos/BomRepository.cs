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
    }
}
