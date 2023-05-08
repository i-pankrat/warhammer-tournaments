using System.ComponentModel.DataAnnotations;

namespace WarhammerTournaments.DAL.Entity;

public interface IEntity
{
    [Key] int Id { get; set; }
    bool IsActive { get; set; }
    DateTime DateCreated { get; set; }
    DateTime? DateUpdated { get; set; }
}