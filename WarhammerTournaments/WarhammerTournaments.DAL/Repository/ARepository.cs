using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Interface;

namespace WarhammerTournaments.DAL.Repository;

public abstract class ARepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
{
    protected readonly ApplicationDbContext _context;

    protected ARepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public abstract Task<TEntity?> Get(TId id);

    public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> selector)
    {
        return await _context.Set<TEntity>().Where(selector).ToListAsync();
    }

    public void Add(TEntity entity)
    {
        // just generate sql
        _context.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _context.Remove(entity);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }
}