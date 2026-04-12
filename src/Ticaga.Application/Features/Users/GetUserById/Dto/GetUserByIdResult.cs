namespace Ticaga.Application.Features.Users.GetUserById.Dto;

public sealed record GetUserByIdResult(
    Guid Id,
    string Email,
    string DisplayName,
    DateTime CreatedUtc);
