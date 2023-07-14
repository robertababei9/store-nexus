using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.GenericRepository
{
    public interface IGenericRepository<T>  where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(Guid id);
        Task<bool> AddAsync(T entity);
        Task<bool> Delete(Guid id);
        Task<bool> Upsert(T entity);

        Task SaveChangesAsync();

        Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>> filterExpr, Expression<Func<T, TResult>> selectExpr);
    }
}
