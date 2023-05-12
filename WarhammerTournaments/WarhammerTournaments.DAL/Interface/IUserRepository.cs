using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Interface;

public interface IUserRepository : IRepository<ApplicationUser, string>
{
    public Task<List<Tournament>> GetAllUserTournaments();
}