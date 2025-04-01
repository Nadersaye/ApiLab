using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace LabApi.Repo
{
    public interface IGenericRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByName(string name);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task PatchAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SoftDeleteAsync(int id);
        Task PatchAsync(int id, JsonPatchDocument<T> patchDoc);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T> GetByNameWithIncludesAsync(string name, params Expression<Func<T, object>>[] includes);
    }
}
