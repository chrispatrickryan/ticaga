namespace Ticaga.Api.Features.Auth.Login;

public sealed record LoginUserResponse(string AccessToken, DateTime ExpiresUtc, Guid UserId, string Email, string DisplayName);
