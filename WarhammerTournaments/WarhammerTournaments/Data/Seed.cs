using Microsoft.AspNetCore.Identity;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.Data.Configurations;

namespace WarhammerTournaments.Data;

public static class Seed
{
    public static async Task SeedRolesAndAdmin(IApplicationBuilder applicationBuilder,
        AdminConfiguration? adminConfiguration)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        
        // Roles
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        if (!await roleManager.RoleExistsAsync(UserRoles.Organizer))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Organizer));
        if (!await roleManager.RoleExistsAsync(UserRoles.User))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        // Admin
        if (adminConfiguration == null)
        {
            return;
        }

        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminUser = await userManager.FindByEmailAsync(adminConfiguration.Email);

        if (adminUser == null)
        {
            var newAdminUser = new ApplicationUser
            {
                UserName = "Pankrat",
                Email = adminConfiguration.Email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(newAdminUser, adminConfiguration.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            }
            else
            {
                throw new InvalidDataException(result.ToString());
            }
        }
    }
}