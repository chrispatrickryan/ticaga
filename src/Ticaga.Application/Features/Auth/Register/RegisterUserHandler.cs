using Ticaga.Application.Abstractions.Security;
using Ticaga.Application.Features.Auth.Register.Dto;
using Ticaga.Domain.Users;

namespace Ticaga.Application.Features.Auth.Register;

public sealed class RegisterUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<OperationResult<RegisterUserResult>> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var errors = RegisterUserCommandValidator.Validate(command);
        if (errors.Count > 0)
        {
            return OperationResult<RegisterUserResult>.ValidationProblem(errors);
        }

        var canonicalEmail = User.CanonicalizeEmail(command.Email);
        var trimmedDisplayName = command.DisplayName.Trim();

        var existingUserByEmail = await _userRepository.GetByEmailAsync(canonicalEmail, cancellationToken);
        if (existingUserByEmail is not null)
        {
            return OperationResult<RegisterUserResult>
                .Conflict("Unable to register with the provided credentials.");
        }

        var displayNameExists = await _userRepository.ExistsByDisplayNameAsync(trimmedDisplayName, cancellationToken);
        if (displayNameExists)
        {
            return OperationResult<RegisterUserResult>
                .Conflict($"A user with the display name '{trimmedDisplayName}' already exists.");
        }

        var user = User.Register(canonicalEmail, trimmedDisplayName, DateTime.UtcNow);

        var passwordHash = _passwordHasher.HashPassword(user, command.Password);
        user.SetPasswordHash(passwordHash);

        try
        {
            await _userRepository.AddAsync(user, cancellationToken);
        }
        catch (Exception)
        {
            return OperationResult<RegisterUserResult>
                .Conflict("Registration failed due to a database issue.");
        }

        return OperationResult<RegisterUserResult>.Success(
            new RegisterUserResult(
                user.Id,
                user.Email,
                user.DisplayName,
                user.CreatedUtc));
    }
}
