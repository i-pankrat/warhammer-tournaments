using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.DAL.Interface;

namespace WarhammerTournaments.DAL.Repository;

public class EntityRepository<TEntity> : ARepository<TEntity, int>, IEntityRepository<TEntity>
    where TEntity : class, IEntity
{
    public EntityRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _context.Set<TEntity>().Where(x => x.IsActive).ToListAsync();
    }

    public override async Task<TEntity?> Get(int id)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id & x.IsActive);
    }

    public async Task Delete(TEntity entity)
    {
        var activatedEntity = await Get(entity.Id);

        if (activatedEntity != null)
        {
            activatedEntity.IsActive = false;
            Update(activatedEntity);
        }
    }
}