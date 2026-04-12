namespace Ticaga.Application.Features.Users.GetCurrentUser.Dto;

public sealed record GetCurrentUserResult(
    Guid Id,
    string Email,
    string DisplayName,
    DateTime CreatedUtc);