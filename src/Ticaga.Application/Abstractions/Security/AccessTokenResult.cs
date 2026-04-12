namespace Ticaga.Application.Abstractions.Security;

public sealed record AccessTokenResult(string AccessToken, DateTime ExpiresUtc);