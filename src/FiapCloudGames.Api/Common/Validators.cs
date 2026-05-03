using System.Text.RegularExpressions;

namespace FiapCloudGames.Api.Common;

public static partial class Validators
{
    public static bool IsValidEmail(string? email) => !string.IsNullOrWhiteSpace(email) && EmailRegex().IsMatch(email);

    public static bool IsStrongPassword(string? password) =>
        !string.IsNullOrWhiteSpace(password)
        && password.Length >= 8
        && password.Any(char.IsLetter)
        && password.Any(char.IsDigit)
        && password.Any(ch => !char.IsLetterOrDigit(ch));

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();
}
