using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infra.SqlServer.Repositories.Base
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected SqlServerContext context { get; }

        protected RepositoryBase(SqlServerContext context)
        {
            this.context = context;
        }

        public async Task<T> GetById(int categoryId)
        {
            return await context.Set<T>().FindAsync(categoryId);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByFilter(Expression<Func<T, bool>> filter)
        {
            return await context.Set<T>().Where(filter).ToListAsync();
        }

        public async Task Add(T entity)
        {
            context.Set<T>().Add(entity);
            //await context.SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
            //await context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            context.Set<T>().Update(entity);
            //await context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            //await context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}
