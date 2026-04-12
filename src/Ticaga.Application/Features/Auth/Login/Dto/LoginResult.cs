namespace Ticaga.Application.Features.Auth.Login.Dto;

public sealed record LoginResult(
    string AccessToken, 
    DateTime ExpiresUtc, 
    Guid UserId, 
    string Email, 
    string DisplayName);
