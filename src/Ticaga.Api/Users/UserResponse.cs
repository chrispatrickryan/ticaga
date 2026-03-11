namespace Ticaga.Api.Users;

public sealed record UserResponse(Guid Id, string DisplayName, DateTime CreatedUtc);
