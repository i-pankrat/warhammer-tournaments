using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Interface;

public interface IUserRepository : IRepository<User, string>
{
    public Task<List<Tournament>> GetAllUserTournaments();
}