namespace Ticaga.Api.Features.Auth.Register;

public sealed record RegisterUserResponse(Guid Id, string Email, string DisplayName, DateTime CreatedUtc);
