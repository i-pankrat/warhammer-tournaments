using WarhammerTournaments.Models;

namespace WarhammerTournaments.Interfaces;

public interface ITournamentRepository
{
    public Task<IEnumerable<Tournament>> GetAll();
    public Task<Tournament> GetByIdAsync(int id);
    public Task<Tournament> GetByIdAsyncNoTracking(int id);
    public bool Add(Tournament tournament);
    public bool Update(Tournament tournament);
    public bool Delete(Tournament tournament);
    public bool Save();
    public Task<IEnumerable<Application>> GetApplicationsByTournamentIdAsync(int id);
    public bool AddApplication(Application application);
    public bool DeleteApplication(Application application);
    public Task<bool> DeleteAllApplicationsByTournamentIdAsync(int id);
    public Task<User> GetUserByIdAsync(string id);
}