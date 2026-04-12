namespace Ticaga.Application.Features.Auth.Register.Dto;

public sealed record RegisterUserCommand(string Email, string DisplayName, string Password);
