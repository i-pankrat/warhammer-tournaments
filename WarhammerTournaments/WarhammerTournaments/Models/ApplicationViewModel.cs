using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.Models;

public class ApplicationViewModel
{
    public IEnumerable<Application> Applications { get; set; }
    public int TournamentId { get; set; }
}