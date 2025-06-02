using ProductionPlanning.Core.Interfaces;
using ProductionPlanning.Core.Model;
using ProductionPlanning.Infrastructure.DbContext;

namespace ProductionPlanning.Infrastructure.Repos
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(InventoryDataContext context) : base(context) { }
    }
}
