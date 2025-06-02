using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IBomRepository Bom { get; }
        public IProductRepository Product { get; }
        public IRawMaterialRepository Material { get; }
        public IUserRepository User { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        int save();
    }
}
