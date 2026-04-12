using Ticaga.Application.Abstractions.Security;
using Ticaga.Application.Features.Auth.Login.Dto;
using Ticaga.Domain.Users;

namespace Ticaga.Application.Features.Auth.Login;

public sealed class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IAccessTokenGenerator accessTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<OperationResult<LoginResult>> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var errors = LoginCommandValidator.Validate(command);
        if (errors.Count > 0)
        {
            return OperationResult<LoginResult>.ValidationProblem(errors);
        }

        var canonicalEmail = User.CanonicalizeEmail(command.Email);

        var user = await _userRepository.GetByEmailAsync(canonicalEmail, cancellationToken);
        if (user is null)
        {
            return OperationResult<LoginResult>.Unauthorized("Invalid email or password.");
        }

        var isPasswordValid = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            command.Password);

        if (!isPasswordValid)
        {
            return OperationResult<LoginResult>.Unauthorized("Invalid email or password.");
        }

        var tokenResult = _accessTokenGenerator.GenerateToken(user);

        var loginResult = new LoginResult(
            tokenResult.AccessToken,
            tokenResult.ExpiresUtc,
            user.Id,
            user.Email,
            user.DisplayName);

        return OperationResult<LoginResult>.Success(loginResult);
    }
}
