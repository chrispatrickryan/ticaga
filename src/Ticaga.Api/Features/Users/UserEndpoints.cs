using Ticaga.Api.Features.Users;
using Ticaga.Domain.Users;

namespace Ticaga.Api.Features.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/users").WithTags("Users");

        group.MapGet("/{id:guid}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithSummary("Retrieves a user by ID.")
            .WithDescription("Retrieves a user by their unique ID. Returns 404 if the user does not exist.")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<IResult> GetUserByIdAsync(
        Guid id,
        IUserRepository userRepository,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return Results.NotFound(new
            {
                Message = $"No user found with ID '{id}'."
            });
        }

        var response = new UserResponse(user.Id, user.DisplayName, user.CreatedUtc);
        return Results.Ok(response);
    }
}