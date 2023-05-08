using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {
    }

    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Application> Applications { get; set; }
    // public DbSet<User> Users { get; set; } -- Defined already
}