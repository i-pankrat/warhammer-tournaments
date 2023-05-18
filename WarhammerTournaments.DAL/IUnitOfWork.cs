using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.DAL.Interface;
using WarhammerTournaments.DAL.Repository;

namespace WarhammerTournaments.DAL;

public interface IUnitOfWork
{
    public TournamentRepository TournamentRepository { get; }
    public IEntityRepository<Application> ApplicationRepository { get; }
    public void Save();
    public Task SaveChangesAsync();
}