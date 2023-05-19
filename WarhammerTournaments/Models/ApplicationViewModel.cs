using System.ComponentModel.DataAnnotations;
using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.Models;

public class ApplicationViewModel
{
    public IEnumerable<Application> Applications { get; set; }
    public int TournamentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Введите положительное число")]
    public int AcceptedPlayerHin { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Введите положительное число")]
    public int AcceptedPlayerElo { get; set; }
}