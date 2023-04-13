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
                    ImageName =
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/5/55/Operation_Sci-Fi_Con_2015.jpg/1920px-Operation_Sci-Fi_Con_2015.jpg",
                    ParticipantNumber = 50,
                    EntranceFee = 100,
                    Date = DateTime.Now.AddDays(2)
                },
                new()
                {
                    Title = "Турнир профессионалов",
                    ImageName =
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/5/55/Operation_Sci-Fi_Con_2015.jpg/1920px-Operation_Sci-Fi_Con_2015.jpg",
                    Description = "ТОП 100 ладера вход",
                    ParticipantNumber = 10,
                    EntranceFee = 1000,
                    Date = DateTime.Now.AddDays(6)
                },
                new()
                {
                    Title = "Средний турнир",
                    ImageName =
                        "https://upload.wikimedia.org/wikipedia/commons/thumb/5/55/Operation_Sci-Fi_Con_2015.jpg/1920px-Operation_Sci-Fi_Con_2015.jpg",
                    Description = "ТОП 500 ладера вход",
                    ParticipantNumber = 20,
                    EntranceFee = 500,
                    Date = DateTime.Now.AddDays(10)
                }
            });
            context.SaveChanges();
        }

        context.SaveChanges();
    }
}