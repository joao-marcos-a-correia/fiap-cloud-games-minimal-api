using FiapCloudGames.Api.Application;
using FiapCloudGames.Api.Common;
using FiapCloudGames.Api.Contracts;
using FiapCloudGames.Api.Domain;
using FiapCloudGames.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fcg.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users").RequireAuthorization();

        group.MapGet("/me/library", async (ClaimsPrincipal principal, FiapGamesDbContext db) =>
        {
            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return await db.UserGames.AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new GameResponse(x.Game.Id, x.Game.Title, x.Game.Description, x.Game.Price))
                .ToListAsync();
        });

        group.MapGet("/", async (FiapGamesDbContext db) =>
            await db.Users.AsNoTracking()
                .Select(x => new UserResponse(x.Id, x.Name, x.Email, x.Role))
                .ToListAsync())
            .RequireAuthorization(policy => policy.RequireRole(UserRole.Admin.ToString()));

        group.MapPost("/", async (CreateUserByAdminRequest request, FiapGamesDbContext db, AuthService auth) =>
        {
            if (!Validators.IsStrongPassword(request.Password))
                return Results.BadRequest(new { error = "Senha deve ter no mínimo 8 caracteres com letras, números e caracteres especiais." });

            var user = new User(request.Name, request.Email, auth.HashPassword(request.Password), request.Role);
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/api/users/{user.Id}", new UserResponse(user.Id, user.Name, user.Email, user.Role));
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Admin.ToString()));

        return app;
    }
}
