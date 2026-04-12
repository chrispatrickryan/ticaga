using Ticaga.Application.Abstractions.Security;
using Ticaga.Domain.Users;

namespace Ticaga.Api.Jwt;

public sealed class JwtAccessTokenGenerator : IAccessTokenGenerator
{
    private readonly IJwtTokenService _jwtTokenService;

    public JwtAccessTokenGenerator(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public AccessTokenResult GenerateToken(User user)
    {
        var tokenResult = _jwtTokenService.GenerateToken(user);

        return new AccessTokenResult(
            tokenResult.AccessToken,
            tokenResult.ExpiresUtc);
    }
}