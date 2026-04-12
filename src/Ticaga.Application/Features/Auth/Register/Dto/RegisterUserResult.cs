namespace Ticaga.Application.Features.Auth.Register.Dto;

public sealed record RegisterUserResult(
    Guid Id, 
    string Email, 
    string DisplayName, 
    DateTime CreatedUtc);
