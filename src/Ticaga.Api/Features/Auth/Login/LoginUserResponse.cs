namespace Ticaga.Api.Features.Auth.Login;

public sealed record LoginUserResponse(Guid Id, string Email, string DisplayName, DateTime CreatedUtc);
