using FiapCloudGames.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Api.Infrastructure;

public sealed class FiapGamesDbContext(DbContextOptions<FiapGamesDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<UserGame> UserGames => Set<UserGame>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(220).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.Role).HasConversion<string>().HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<Game>(builder =>
        {
            builder.ToTable("Games");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).HasMaxLength(180).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<UserGame>(builder =>
        {
            builder.ToTable("UserGames");

            builder.HasKey(x => new { x.UserId, x.GameId });
            builder.HasOne(x => x.User).WithMany(x => x.Library).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Game).WithMany(x => x.Owners).HasForeignKey(x => x.GameId);
        });
    }
}
