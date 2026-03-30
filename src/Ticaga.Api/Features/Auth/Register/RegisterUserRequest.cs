namespace Ticaga.Api.Features.Auth.Register;

public sealed record RegisterUserRequest(string Email, string DisplayName, string Password);