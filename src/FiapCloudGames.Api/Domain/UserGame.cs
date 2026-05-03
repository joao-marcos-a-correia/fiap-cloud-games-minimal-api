
using FiapCloudGames.Api.Common;

namespace FiapCloudGames.Api.Domain;

public sealed class UserGame
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid GameId { get; private set; }
    public Game Game { get; private set; } = null!;
    public DateTime PurchasedAtUtc { get; private set; } = DateTime.UtcNow;

    private UserGame() { }

    public UserGame(Guid userId, Guid gameId)
    {
        if (userId == Guid.Empty || gameId == Guid.Empty) throw new DomainException("Usuário e jogo são obrigatórios.");
        UserId = userId;
        GameId = gameId;
    }
}
