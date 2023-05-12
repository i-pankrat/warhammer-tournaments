using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.Models;

public class ApplicationUsersViewModel
{
    public IEnumerable<ApplicationUser> Users { get; set; }
}