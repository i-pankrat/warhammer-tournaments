using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarhammerTournaments.Models;

public class Application
{
    [Key] public int Id { get; set; }
    [ForeignKey("Tournament")] public int TournamentId { get; set; }
    [ForeignKey("User")] public string UserId { get; set; }
    public string UserName { get; set; }
    public int Elo { get; set; }
    public int Hin { get; set; }
    public string Fraction { get; set; }
    public string Roster { get; set; }
}