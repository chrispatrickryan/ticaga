namespace Ticaga.Api.Features.Auth.Jwt;

public sealed record CurrentUserResponse(Guid Id, string Email, string DisplayName, DateTime CreatedUtc);
