using Ticaga.Domain.Users;

namespace Ticaga.Api.Features.Auth.Jwt;

public interface IJwtTokenService
{
    JwtTokenResult GenerateToken(User user);
}