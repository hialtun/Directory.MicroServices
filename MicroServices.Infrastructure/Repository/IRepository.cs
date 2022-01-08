using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroServices.Infrastructure.Repository
{
    public interface IRepository<TDocument> where TDocument : class
    {
        IQueryable<TDocument> Get(Expression<Func<TDocument, bool>> predicate = null);
        Task<TDocument> GetByIdAsync(string id);
        Task<TDocument> CreateAsync(TDocument entity);
        Task<TDocument> UpdateAsync(TDocument entity);
        Task<TDocument> DeleteAsync(string id);
    }
}