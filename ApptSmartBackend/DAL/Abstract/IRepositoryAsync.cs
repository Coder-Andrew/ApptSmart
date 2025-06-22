using System.Linq.Expressions;

namespace ApptSmartBackend.DAL.Abstract
{
    public interface IRepositoryAsync<TEntity> where TEntity : class, new()
    {
        Task<TEntity?> FindByIdAsync(object id);
        Task<bool> ExistsAsync(object id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicates);
        Task<IList<TEntity>> GetAllAsync();
        Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, bool>>[] predicates);
        Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] predicates);
        Task<TEntity> AddOrUpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteByIdAsync(object id);
        
    }
}
