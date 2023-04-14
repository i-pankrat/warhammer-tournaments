using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.Data;
using WarhammerTournaments.Interfaces;
using WarhammerTournaments.Models;

namespace WarhammerTournaments.Repository;

public class TournamentRepository : ITournamentRepository
{
    private readonly ApplicationDbContext _context;

    public TournamentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tournament>> GetAll()
    {
        return await _context.Tournaments.ToListAsync();
    }

    public async Task<Tournament> GetByIdAsync(int id)
    {
        return await _context.Tournaments.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Tournament> GetByIdAsyncNoTracking(int id)
    {
        return await _context.Tournaments.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }

    public bool Add(Tournament tournament)
    {
        // just generate sql
        _context.Add(tournament);
        // send sql to the data base
        return Save();
    }

    public bool Update(Tournament tournament)
    {
        _context.Update(tournament);
        return Save();
    }

    public bool Delete(Tournament tournament)
    {
        _context.Remove(tournament);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}