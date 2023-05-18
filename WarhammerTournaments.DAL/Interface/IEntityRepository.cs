using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Interface;

public interface IEntityRepository<TEntity> : IDeletableRepository<TEntity, int> where TEntity : class, IEntity
{
}