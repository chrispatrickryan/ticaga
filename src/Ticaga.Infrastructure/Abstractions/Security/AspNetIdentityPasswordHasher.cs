using Microsoft.AspNetCore.Identity;
using Ticaga.Application.Abstractions.Security;
using Ticaga.Domain.Users;

namespace Ticaga.Infrastructure.Abstractions.Security;

public sealed class AspNetIdentityPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _netCorePasswordHasher = new();

    public string HashPassword(User user, string password)
    {
        return _netCorePasswordHasher.HashPassword(user, password);
    }

    public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        var result = _netCorePasswordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result != PasswordVerificationResult.Failed;
    }
}