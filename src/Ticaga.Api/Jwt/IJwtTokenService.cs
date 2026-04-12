using Ticaga.Api.Jwt.Dto;
using Ticaga.Domain.Users;

namespace Ticaga.Api.Jwt;

public interface IJwtTokenService
{
    JwtTokenResult GenerateToken(User user);
}