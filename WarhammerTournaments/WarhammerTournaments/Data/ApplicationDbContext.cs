using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.Models;

namespace WarhammerTournaments.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {
    }

    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<User> Users { get; set; }
}