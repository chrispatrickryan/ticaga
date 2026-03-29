namespace Ticaga.Api.Features.Users;

public sealed record UserResponse(Guid Id, string DisplayName, DateTime CreatedUtc);
