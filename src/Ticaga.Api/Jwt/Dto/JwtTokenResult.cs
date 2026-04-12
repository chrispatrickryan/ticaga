namespace Ticaga.Api.Jwt.Dto;

public sealed record JwtTokenResult(string AccessToken, DateTime ExpiresUtc);