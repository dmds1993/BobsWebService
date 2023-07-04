using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T category);
        Task Update(T category);
        Task Delete(T category);
        Task<IEnumerable<T>> GetByFilter(Expression<Func<T, bool>> filter);
        Task AddRange(IEnumerable<T> entities);
        Task SaveChanges();
    }
}
