using FiapCloudGames.Api.Common;

namespace FiapCloudGames.Api.Domain;

public sealed class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; } = UserRole.User;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public List<UserGame> Library { get; private set; } = [];

    private User() { }

    public User(string name, string email, string passwordHash, UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Nome é obrigatório.");
        if (!Validators.IsValidEmail(email)) throw new DomainException("E-mail inválido.");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new DomainException("Senha criptografada é obrigatória.");

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
    }
}
