using System.Linq.Expressions;
using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Interface;

public interface IRepository<TEntity, in TId> where TEntity : class
{
    public Task<IEnumerable<TEntity>> GetAll();
    public Task<TEntity?> Get(TId id);
    public Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> selector);

    // CRUD
    public void Add(TEntity entity);
    public void Update(TEntity entity);
    public void Remove(TEntity entity);

    // CRUD async
    public Task AddAsync(TEntity entity);
    // public Task UpdateAsync(T model);
    // public Task RemoveAsync(T model);

    // Range operations
    public void AddRange(IEnumerable<TEntity> entities);

    public void RemoveRange(IEnumerable<TEntity> entities);
    // public void DeleteRange(IEnumerable<T> entities);

    // Range operations async
    public Task AddRangeAsync(IEnumerable<TEntity> models);
    // public Task RemoveRangeAsync(IEnumerable<T> models);
    // public Task DeleteRangeAsync(IEnumerable<T> models);
}