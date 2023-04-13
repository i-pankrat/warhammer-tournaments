using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.Models;

namespace WarhammerTournaments.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {
    }

    public DbSet<Tournament> Tournaments { get; set; }
}