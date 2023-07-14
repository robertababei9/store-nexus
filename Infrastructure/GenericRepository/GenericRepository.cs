using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext context;
        internal DbSet<T> dbSet;
        //protected readonly ILogger _logger;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
            //_logger = logger;
        }

        public async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<bool> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public virtual async Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>> filterExpr, Expression<Func<T, TResult>> selectExpr)
        {
            return await dbSet
                        .Where(filterExpr)
                        .Select(selectExpr)
                        .FirstOrDefaultAsync();
        }
    }
}
