using FiapCloudGames.Api.Application;
using FiapCloudGames.Api.Common;
using FiapCloudGames.Api.Contracts;
using FiapCloudGames.Api.Domain;
using FiapCloudGames.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterUserRequest request, FiapGamesDbContext db, AuthService auth) =>
        {
            if (!Validators.IsStrongPassword(request.Password))
                return Results.BadRequest(new { error = "Senha deve ter no mínimo 8 caracteres com letras, números e caracteres especiais." });

            var exists = await db.Users.AnyAsync(x => x.Email == request.Email.ToLower());
            if (exists) return Results.Conflict(new { error = "E-mail já cadastrado." });

            var user = new User(request.Name, request.Email, auth.HashPassword(request.Password));
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/api/users/{user.Id}", new UserResponse(user.Id, user.Name, user.Email, user.Role));
        });

        group.MapPost("/login", async (LoginRequest request, FiapGamesDbContext db, AuthService auth) =>
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await db.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user is null || !auth.VerifyPassword(request.Password, user.PasswordHash))
                return Results.Unauthorized();

            return Results.Ok(new AuthResponse(auth.GenerateToken(user)));
        });

        return app;
    }
}
