using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarhammerTournaments.Models;

public class Tournament
{
    [Key] public int Id { get; set; }
    [ForeignKey("User")] public string? UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ParticipantNumber { get; set; }
    public DateTime Date { get; set; }
    public string ImageName { get; set; }
    public int EntranceFee { get; set; }
}