using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarhammerTournaments.Models;

public class Tournament
{
    [Key] public int Id { get; set; }
    [ForeignKey("User")] public string? OwnerId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AvailableParticipant { get; set; }
    public int RegisteredParticipant { get; set; }
    public List<User> Users { get; set; }
    public string Address { get; set; }
    public DateTime Date { get; set; }
    public string ImageUrl { get; set; }
    public string ImageId { get; set; }
    public int EntranceFee { get; set; }
}