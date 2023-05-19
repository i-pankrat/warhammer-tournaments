using System.ComponentModel.DataAnnotations;

namespace WarhammerTournaments.Models;

public class JoinViewModel
{
    public string UserId { get; set; }
    public int TournamentId { get; set; }

    [Required(ErrorMessage = "Введите название вашей фракции")]
    public string Fraction { get; set; }

    [Required(ErrorMessage = "Введите ваш ростер")]
    public string Roster { get; set; }
}