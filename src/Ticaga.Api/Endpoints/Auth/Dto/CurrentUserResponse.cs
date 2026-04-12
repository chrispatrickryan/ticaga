namespace Ticaga.Api.Endpoints.Auth.Dto;

public sealed record CurrentUserResponse(
    Guid Id,
    string Email,
    string DisplayName,
    DateTime CreatedUtc);