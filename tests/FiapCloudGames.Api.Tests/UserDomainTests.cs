using FiapCloudGames.Api.Common;
using FiapCloudGames.Api.Domain;
using Xunit;

namespace Fcg.Api.Tests;

public sealed class UserDomainTests
{
    [Fact]
    public void Should_create_user_with_normalized_email()
    {
        var user = new User("Joao", "JOAO@EMAIL.COM", "hash");

        Assert.Equal("joao@email.com", user.Email);
        Assert.Equal(UserRole.User, user.Role);
    }

    [Fact]
    public void Should_reject_invalid_email()
    {
        Assert.Throws<DomainException>(() => new User("Joao", "invalid", "hash"));
    }
}
