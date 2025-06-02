using Microsoft.EntityFrameworkCore.Storage;
using ProductionPlanning.Core.Interfaces;
using ProductionPlanning.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Infrastructure.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;
        private readonly InventoryDataContext _context;
        public IBomRepository Bom { get; }
        public IProductRepository Product { get; }
        public IRawMaterialRepository Material { get; }
        public IUserRepository User { get; }
        public UnitOfWork( InventoryDataContext context, IBomRepository bom, IProductRepository product, IRawMaterialRepository material, IUserRepository user)
        {
            _context = context;
            Bom = bom;
            Product = product;
            Material = material;
            User = user;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int save()
        {
            return _context.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
            return _currentTransaction;
        }

        public async Task CommitAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                _currentTransaction = null;
            }
        }
    }
}
