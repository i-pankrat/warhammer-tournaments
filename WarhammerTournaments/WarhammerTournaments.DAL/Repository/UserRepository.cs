using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.DAL.Interface;

namespace WarhammerTournaments.DAL.Repository;

public class UserRepository : ARepository<ApplicationUser, string>, IUserRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<ApplicationUser?> Get(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Tournament>> GetAllUserTournaments()
    {
        var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        var userTournaments = _context.Tournaments.Where(t => t.OwnerId == curUserId && t.IsActive);
        return await userTournaments.ToListAsync();
    }
}