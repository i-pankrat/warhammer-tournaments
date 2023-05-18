using System.ComponentModel.DataAnnotations;

namespace WarhammerTournaments.ViewModels;

public class TournamentViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Rules { get; set; }
    public string Address { get; set; }
    public string OwnerId { get; set; }
    public string OwnerUserName { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Введите положительное число")]
    public int AvailableParticipant { get; set; }

    public DateTime Date { get; set; }
    public IFormFile? Image { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Введите неотрицательное число")]
    public int EntranceFee { get; set; }
}