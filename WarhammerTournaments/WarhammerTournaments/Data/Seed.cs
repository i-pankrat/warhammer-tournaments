using Microsoft.AspNetCore.Identity;
using WarhammerTournaments.Models;

namespace WarhammerTournaments.Data;

public static class Seed
{
    public static void SeedData(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Database.EnsureCreated();

        if (!context.Tournaments.Any())
        {
            context.Tournaments.AddRange(new List<Tournament>()
            {
                new()
                {
                    Title = "Новичковый турнир",
                    Description = "Приглашаю вас принять участие в новом турнире новичковой лиги!",
                    ImageUrl =
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/5/55/Operation_Sci-Fi_Con_2015.jpg/1920px-Operation_Sci-Fi_Con_2015.jpg",
                    Address = "Улица Пушкина Дом Калатушкино",
                    AvailableParticipant = 50,
                    RegisteredParticipant = 10,
                    EntranceFee = 100,
                    Date = DateTime.Now.AddDays(2)
                },
                new()
                {
                    Title = "Турнир профессионалов",
                    ImageUrl = 
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/5/55/Operation_Sci-Fi_Con_2015.jpg/1920px-Operation_Sci-Fi_Con_2015.jpg",
                    Address = "Улица Пушкина Дом Калатушкино",
                    Description = "ТОП 100 ладера вход",
                    AvailableParticipant = 10,
                    RegisteredParticipant = 2,
                    EntranceFee = 1000,
                    Date = DateTime.Now.AddDays(6)
                },
                new()
                {
                    Title = "Средний турнир",
                    ImageUrl =
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/5/55/Operation_Sci-Fi_Con_2015.jpg/1920px-Operation_Sci-Fi_Con_2015.jpg",
                    Address = "Улица Пушкина Дом Калатушкино",
                    Description = "ТОП 500 ладера вход",
                    AvailableParticipant = 20,
                    RegisteredParticipant = 0,
                    EntranceFee = 500,
                    Date = DateTime.Now.AddDays(10)
                }
            });
            context.SaveChanges();
        }

        context.SaveChanges();
    }

    public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
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

        var devPassword = "@Development_Password_1@";

        // Users
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var adminEmail = "admin@testpankrat.com";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newAdminUser = new User
            {
                UserName = "pankratadmin",
                Email = adminEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(newAdminUser, devPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            }
            else
            {
                throw new InvalidDataException(result.ToString());
            }
        }

        var organizerEmail = "organizer@testpankrat.com";

        var organizer = await userManager.FindByEmailAsync(organizerEmail);
        if (organizer == null)
        {
            var newOrganizerUser = new User
            {
                UserName = "pankratorganizer",
                Email = organizerEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(newOrganizerUser, devPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newOrganizerUser, UserRoles.Admin);
            }
            else
            {
                throw new InvalidDataException(result.ToString());
            }
        }

        var userEmail = "user@testpankrat.com";

        var appUser = await userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            var newAppUser = new User
            {
                UserName = "pankratuser",
                Email = userEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(newAppUser, devPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAppUser, UserRoles.Admin);
            }
            else
            {
                throw new InvalidDataException(result.ToString());
            }

            await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
        }
    }
}