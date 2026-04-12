namespace Ticaga.Api.Endpoints.Auth.Dto;

public sealed record RegisterUserRequest(string Email, string DisplayName, string Password);
