using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Repository;

public class ApplicationRepository : EntityRepository<Application>
{
    public ApplicationRepository(ApplicationDbContext context) : base(context)
    {
    }
}