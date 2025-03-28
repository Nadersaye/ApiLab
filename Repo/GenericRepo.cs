using LabApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabApi.Repo
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly ITIDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepo(ITIDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetByName(string name)
        {
            return await _dbSet.FindAsync(name);
        }

        public async Task PatchAsync(int id, JsonPatchDocument<T> patchDoc)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found");
            }

            patchDoc.ApplyTo(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found");
            }

            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
            if (isDeletedProperty == null)
            {
                throw new InvalidOperationException("IsDeleted property not found on entity type");
            }

            isDeletedProperty.SetValue(entity, true);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public Task PatchAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
