using Ticaga.Application.Features.Users.GetCurrentUser.Dto;
using Ticaga.Domain.Users;

namespace Ticaga.Application.Features.Users.GetCurrentUser;

public sealed class GetCurrentUserHandler
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OperationResult<GetCurrentUserResult>> HandleAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            return OperationResult<GetCurrentUserResult>.Unauthorized("Invalid authenticated user.");
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return OperationResult<GetCurrentUserResult>.Unauthorized("Authenticated user was not found.");
        }

        return OperationResult<GetCurrentUserResult>.Success(
            new GetCurrentUserResult(
                user.Id,
                user.Email,
                user.DisplayName,
                user.CreatedUtc));
    }
}