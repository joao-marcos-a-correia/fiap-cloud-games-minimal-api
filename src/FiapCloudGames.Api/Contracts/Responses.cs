using FiapCloudGames.Api.Domain;

namespace FiapCloudGames.Api.Contracts;

public sealed record AuthResponse(string AccessToken);
public sealed record UserResponse(Guid Id, string Name, string Email, UserRole Role);
public sealed record GameResponse(Guid Id, string Title, string Description, decimal Price);
