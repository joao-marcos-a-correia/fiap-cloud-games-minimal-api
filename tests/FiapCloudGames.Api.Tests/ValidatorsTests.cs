using FiapCloudGames.Api.Common;
using Xunit;

namespace Fcg.Api.Tests;

public sealed class ValidatorsTests
{
    [Theory]
    [InlineData("joao@email.com")]
    [InlineData("user.name+tag@domain.com")]
    public void Should_accept_valid_email(string email)
    {
        Assert.True(Validators.IsValidEmail(email));
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("@domain.com")]
    public void Should_reject_invalid_email(string email)
    {
        Assert.False(Validators.IsValidEmail(email));
    }

    [Fact]
    public void Should_require_strong_password()
    {
        Assert.True(Validators.IsStrongPassword("Senha@123"));
        Assert.False(Validators.IsStrongPassword("senha123"));
    }
}
