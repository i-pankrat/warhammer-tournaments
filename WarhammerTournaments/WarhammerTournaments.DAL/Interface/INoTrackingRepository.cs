using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Interface;

public interface INoTrackingRepository<T, in TId> : IRepository<T, TId> where T : class
{
    public Task<T?> GetNoTracking(T entity);
    public Task<T?> GetNoTracking(TId id);
}