using Microsoft.EntityFrameworkCore;
using ProductionPlanning.Core.Interfaces;
using ProductionPlanning.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPlanning.Infrastructure.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected InventoryDataContext context;

        public GenericRepository(InventoryDataContext context1)
        {
            this.context = context1;
        }

        public async Task Add(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            var result = await this.context.Set<T>().FindAsync(id);
            if (result != null)
            {
                return result;
            }
            return Activator.CreateInstance<T>();
        }

        public void Update(T entity)
        {
            this.context.Set<T>().Update(entity);
        }
    }
}
