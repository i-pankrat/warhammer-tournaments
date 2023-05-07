using System.Collections;
using WarhammerTournaments.Models;

namespace WarhammerTournaments.Interfaces;

public interface ITournamentRepository
{
    // Tournaments
    public Task<IEnumerable<Tournament>> GetAll();
    public Task<Tournament> GetByIdAsync(int id);
    public Task<Tournament> GetByIdWithApplicationsAsync(int id);
    public Task<Tournament> GetByIdAsyncNoTracking(int id);
    public bool Add(Tournament tournament);
    public bool Update(Tournament tournament);
    public bool Delete(Tournament tournament);
    public bool Save();
    
    // Applications
    public Task<Application> GetApplicationByIdAsync(int id);
    public Task<IEnumerable<Application>> GetAcceptedApplicationsByTournamentIdAsync(int id);
    public Task<IEnumerable<Application>> GetNotAcceptedApplicationsByTournamentIdAsync(int id);
    public Task<IEnumerable<Application>> GetApplicationsByTournamentIdAsync(int id);
    public bool AddApplication(Application application);
    public bool DeleteApplication(Application application);
    public bool UpdateApplication(Application application);
    public Task<bool> DeleteAllApplicationsByTournamentIdAsync(int id);
    
    // User
    public Task<User> GetUserByIdAsync(string id);
}