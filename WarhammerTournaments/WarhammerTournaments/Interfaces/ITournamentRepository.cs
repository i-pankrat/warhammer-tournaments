using WarhammerTournaments.Models;

namespace WarhammerTournaments.Interfaces;

public interface ITournamentRepository
{
    public Task<IEnumerable<Tournament>> GetAll();
    public Task<Tournament> GetByIdAsync(int id);
    public bool Add(Tournament tournament);
    public bool Update(Tournament tournament);
    public bool Delete(Tournament tournament);
    public bool Save();
}