using Gyanisa.Data.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gyanisa.Data.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext _applicationDbContext { get; set; }
        public RepositoryBase(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public int Count(Func<T, bool> predicate)
        {
            return _applicationDbContext.Set<T>().Where(predicate).Count();
        }

        public void Create(T entity)
        {
            this._applicationDbContext.Set<T>()
                .Add(entity);
        }

        public void Delete(T entity)
        {
            this._applicationDbContext.Set<T>()
                .Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await this._applicationDbContext.Set<T>()
                .Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this._applicationDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Set<T>().FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await this._applicationDbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            this._applicationDbContext.Set<T>().Update(entity);
        }
    }
}
