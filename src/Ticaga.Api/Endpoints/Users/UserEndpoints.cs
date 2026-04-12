using Ticaga.Application.Common;
using Ticaga.Application.Features.Users.GetUserById;
using Ticaga.Application.Features.Users.GetUserById.Dto;

namespace Ticaga.Api.Endpoints.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/users").WithTags("Users");

        group.MapGet("/{id:guid}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithSummary("Retrieves a user by ID.")
            .WithDescription("Retrieves a user by their unique ID. Returns 404 if the user does not exist.")
            .Produces<GetUserByIdResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<IResult> GetUserByIdAsync(
        Guid id,
        GetUserByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                TicagaErrorType.NotFound => Results.NotFound(result.ErrorMessage),
                _ => Results.Problem(result.ErrorMessage)
            };
        }

        return Results.Ok(result.Value);
    }
}