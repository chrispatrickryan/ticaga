namespace Ticaga.Api.Endpoints.Auth.Dto;

public sealed record LoginResponse(
    string AccessToken, 
    DateTime ExpiresUtc, 
    Guid UserId, 
    string Email, 
    string DisplayName);
