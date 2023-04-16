using WarhammerTournaments.Data;
using WarhammerTournaments.Interfaces;
using WarhammerTournaments.Models;

namespace WarhammerTournaments.Repository;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Tournament>> GetAllUserTournaments()
    {
        var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        var userTournaments = _context.Tournaments.Where(t => t.UserId == curUserId);
        return userTournaments.ToList();
    }
}