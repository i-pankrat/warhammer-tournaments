using WarhammerTournaments.Models;

namespace WarhammerTournaments.ViewModels;

public class ApplicationViewModel
{
    public IEnumerable<Application> Applications { get; set; }
    public int TournamentId { get; set; }
}