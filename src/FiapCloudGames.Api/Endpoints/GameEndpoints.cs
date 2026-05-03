using FiapCloudGames.Api.Contracts;
using FiapCloudGames.Api.Domain;
using FiapCloudGames.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fcg.Api.Endpoints;

public static class GameEndpoints
{
    public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/games").WithTags("Games").RequireAuthorization();

        group.MapGet("/", async (FiapGamesDbContext db) =>
            await db.Games.AsNoTracking()
                .Select(x => new GameResponse(x.Id, x.Title, x.Description, x.Price))
                .ToListAsync());

        group.MapPost("/", async (CreateGameRequest request, FiapGamesDbContext db) =>
        {
            var game = new Game(request.Title, request.Description, request.Price);
            db.Games.Add(game);
            await db.SaveChangesAsync();
            return Results.Created($"/api/games/{game.Id}", new GameResponse(game.Id, game.Title, game.Description, game.Price));
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Admin.ToString()));

        group.MapPost("/purchase", async (PurchaseGameRequest request, ClaimsPrincipal principal, FiapGamesDbContext db) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (!await db.Games.AnyAsync(x => x.Id == request.GameId)) return Results.NotFound(new { error = "Jogo não encontrado." });
            if (await db.UserGames.AnyAsync(x => x.UserId == userId && x.GameId == request.GameId)) return Results.Conflict(new { error = "Jogo já está na biblioteca." });

            db.UserGames.Add(new UserGame(userId, request.GameId));
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return app;
    }
}
