using WarhammerTournaments.Models;

namespace WarhammerTournaments.Interfaces;

public interface IDashboardRepository
{
    public Task<List<Tournament>> GetAllUserTournaments();
}