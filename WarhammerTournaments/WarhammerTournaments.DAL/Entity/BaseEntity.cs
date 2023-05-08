using System.ComponentModel.DataAnnotations;

namespace WarhammerTournaments.DAL.Entity;

public class BaseEntity : IEntity
{
    [Key] public int Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}