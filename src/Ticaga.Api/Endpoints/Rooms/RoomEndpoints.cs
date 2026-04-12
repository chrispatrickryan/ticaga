using Ticaga.Api.Endpoints.Rooms.Dto;
using Ticaga.Application.Common;
using Ticaga.Application.Features.Rooms.CreateRoom;
using Ticaga.Application.Features.Rooms.CreateRoom.Dto;
using Ticaga.Application.Features.Rooms.GetRoomById;
using Ticaga.Application.Features.Rooms.GetRoomById.Dto;

namespace Ticaga.Api.Endpoints.Rooms;

public static class RoomEndpoints
{
    public static IEndpointRouteBuilder MapRoomEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/rooms").WithTags("Rooms");

        group.MapPost("/", CreateRoomAsync)
            .WithName("CreateRoom")
            .WithSummary("Creates a new room.")
            .WithDescription("Creates a new room with a unique display name and saves it to the database.")
            .Produces<CreateRoomResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapGet("/{id:guid}", GetRoomByIdAsync)
            .WithName("GetRoomById")
            .WithSummary("Retrieves a room by ID.")
            .WithDescription("Retrieves a room by its unique ID. Returns 404 if the room does not exist.")
            .Produces<GetRoomByIdResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<IResult> CreateRoomAsync(
        CreateRoomRequest request,
        CreateRoomHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateRoomCommand(request.Name, request.HostUserId);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                TicagaErrorType.Validation => Results.ValidationProblem(result.ValidationErrors!),
                TicagaErrorType.Conflict => Results.Conflict(result.ErrorMessage),
                _ => Results.Problem(result.ErrorMessage)
            };
        }

        var response = new CreateRoomResponse(
            result.Value!.Id,
            result.Value.Name,
            result.Value.HostUserId,
            result.Value.Status.ToString(),
            result.Value.CreatedUtc);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetRoomByIdAsync(
        Guid id,
        GetRoomByIdHandler handler,
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

        var response = new CreateRoomResponse(
            result.Value!.Id,
            result.Value.Name,
            result.Value.HostUserId,
            result.Value.Status.ToString(),
            result.Value.CreatedUtc);

        return Results.Ok(response);
    }
}
