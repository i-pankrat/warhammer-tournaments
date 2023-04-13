namespace WarhammerTournaments.ViewModels;

public class TournamentViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int ParticipantNumber { get; set; }
    public DateTime Date { get; set; }
    public IFormFile Image { get; set; }
    public int EntranceFee { get; set; }
}