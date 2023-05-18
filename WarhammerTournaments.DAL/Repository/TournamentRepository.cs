using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.DAL.Interface;

namespace WarhammerTournaments.DAL.Repository;

public class TournamentRepository : EntityRepository<Tournament>, INoTrackingRepository<Tournament, int>
{
    public TournamentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Tournament?> GetNoTracking(Tournament entity)
    {
        return await _context.Tournaments.AsNoTracking().FirstOrDefaultAsync(i => i.Id == entity.Id);
    }

    public async Task<Tournament?> GetNoTracking(int id)
    {
        return await _context.Tournaments.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }
}