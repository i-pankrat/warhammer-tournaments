using System.ComponentModel.DataAnnotations.Schema;

namespace WarhammerTournaments.DAL.Entity;

public class Tournament : BaseEntity
{
    // Owner data
    public string OwnerUserName { get; set; }
    [ForeignKey("User")] public string OwnerId { get; set; }

    // Tournament data
    public string Title { get; set; }
    public string Description { get; set; }
    public int AvailableParticipant { get; set; }
    public int RegisteredParticipant { get; set; }
    public string Address { get; set; }
    public DateTime Date { get; set; }
    public int EntranceFee { get; set; }
    public string ImageUrl { get; set; }
    public string ImageId { get; set; }

    // Participant data
    public List<Application> Participants { get; set; }
}