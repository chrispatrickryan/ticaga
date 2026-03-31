namespace Ticaga.Api.Features.Auth.Jwt;

public sealed record JwtTokenResult(string AccessToken, DateTime ExpiresUtc);