using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.DAL.Entity;

namespace WarhammerTournaments.DAL.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {
    }

    public DbSet<Tournament> Tournaments { get; set; }

    public DbSet<Application> Applications { get; set; }
    // public DbSet<User> Users { get; set; } -- Defined already

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole()
            {
                Name = Data.UserRoles.Admin, ConcurrencyStamp = "1",
                NormalizedName = Data.UserRoles.Admin
            },
            new IdentityRole()
            {
                Name = Data.UserRoles.Organizer, ConcurrencyStamp = "2",
                NormalizedName = Data.UserRoles.Organizer
            }, new IdentityRole()
            {
                Name = Data.UserRoles.User, ConcurrencyStamp = "3",
                NormalizedName = Data.UserRoles.User
            });
    }
}