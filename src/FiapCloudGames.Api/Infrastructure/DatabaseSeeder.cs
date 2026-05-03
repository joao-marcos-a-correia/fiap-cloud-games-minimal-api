using FiapCloudGames.Api.Application;
using FiapCloudGames.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Api.Infrastructure;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FiapGamesDbContext>();
        var auth = scope.ServiceProvider.GetRequiredService<AuthService>();
        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync(x => x.Email == "admin@fcg.com"))
        {
            db.Users.Add(new User("Administrador", "admin@fcg.com", auth.HashPassword("Admin@123"), UserRole.Admin));
        }

        if (!await db.Games.AnyAsync())
        {
            db.Games.AddRange(
                new Game("Clean Code", "Jogo teste.", 49.90m),
                new Game("Cloud Architect", "Jogo teste.", 89.90m)
            );
        }

        await db.SaveChangesAsync();
    }
}
