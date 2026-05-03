using FiapCloudGames.Api.Domain;

namespace FiapCloudGames.Api.Contracts;

public sealed record RegisterUserRequest(string Name, string Email, string Password);
public sealed record LoginRequest(string Email, string Password);
public sealed record CreateGameRequest(string Title, string Description, decimal Price);
public sealed record PurchaseGameRequest(Guid GameId);
public sealed record CreateUserByAdminRequest(string Name, string Email, string Password, UserRole Role);
