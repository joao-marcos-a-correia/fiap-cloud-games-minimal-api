
using FiapCloudGames.Api.Common;

namespace FiapCloudGames.Api.Domain;

public sealed class Game
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public List<UserGame> Owners { get; private set; } = [];

    private Game() { }

    public Game(string title, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new DomainException("Título do jogo é obrigatório.");
        if (price < 0) throw new DomainException("Preço não pode ser negativo.");
        Title = title.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
    }
}
