using ApptSmartBackend.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApptSmartBackend.DAL.Concrete
{
    public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, new()
    {
        private readonly DbSet<TEntity> _dbSet;
        protected readonly DbContext _context;
        public RepositoryAsync(DbContext ctx)
        {
            _context = ctx;
            _dbSet = _context.Set<TEntity>();
        }
        public virtual async Task<TEntity> AddOrUpdateAsync(TEntity entity)
        {
            var entry = _context.Entry(entity);

            if (entry.IsKeySet)
            {
                _dbSet.Update(entity);
            }
            else
            {
                await _dbSet.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteByIdAsync(object id)
        {
            var entity = await FindByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(object id)
        {
            TEntity? entity = await FindByIdAsync(id);
            return entity != null;
        }

        public virtual async Task<TEntity?> FindByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (Expression<Func<TEntity, bool>> expression in predicates)
            {
                query = query.Where(expression);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] predicates)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (Expression<Func<TEntity, object>> expression in predicates)
            {
                query = query.Include(expression);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
