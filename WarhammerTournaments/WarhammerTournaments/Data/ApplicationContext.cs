using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.Models;

namespace WarhammerTournaments.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> option) : base(option)
    {
    }

    public DbSet<Tournament> Tournaments { get; set; }
}