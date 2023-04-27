namespace WarhammerTournaments.ViewModels;

public class TournamentViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string? OwnerId { get; set; }
    public int AvailableParticipant { get; set; }
    public DateTime Date { get; set; }
    public IFormFile? Image { get; set; }
    public int EntranceFee { get; set; }
}