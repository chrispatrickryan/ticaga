using Ticaga.Application.Features.Users.GetUserById.Dto;
using Ticaga.Domain.Users;

namespace Ticaga.Application.Features.Users.GetUserById;

public sealed class GetUserByIdHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OperationResult<GetUserByIdResult>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return OperationResult<GetUserByIdResult>.NotFound($"User with id {id} not found.");
        }

        return OperationResult<GetUserByIdResult>.Success(
            new GetUserByIdResult(
                user.Id, 
                user.Email, 
                user.DisplayName, 
                user.CreatedUtc));
    }
}
