using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Interface;

public interface IDeletableRepository<T, in TId> : IRepository<T, TId> where T : class
{
    // Make model inactive
    public Task Delete(T entity);
}