using System.ComponentModel.DataAnnotations.Schema;

namespace WarhammerTournaments.DAL.Entity;

public class Application : BaseEntity
{
    // User information
    public User User { get; set; }
    [ForeignKey("User")] public string UserId { get; set; }
    public string UserName { get; set; }
    public int Elo { get; set; }
    public int Hin { get; set; }

    // Tournament information
    public Tournament Tournament { get; set; }
    [ForeignKey("Tournament")] public int TournamentId { get; set; }

    // Application information
    public bool IsAccepted { get; set; }
    public string Fraction { get; set; }
    public string Roster { get; set; }
}