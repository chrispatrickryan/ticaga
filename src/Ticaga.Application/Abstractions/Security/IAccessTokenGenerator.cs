using Ticaga.Domain.Users;

namespace Ticaga.Application.Abstractions.Security;

public interface IAccessTokenGenerator
{
    AccessTokenResult GenerateToken(User user);
}