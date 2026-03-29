using Ticaga.Domain.Rooms;
using Ticaga.Domain.Users;

namespace Ticaga.Api.Features.Rooms;

public static class RoomEndpoints
{
    public static IEndpointRouteBuilder MapRoomEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/rooms").WithTags("Rooms");

        group.MapPost("/", CreateRoomAsync)
            .WithName("CreateRoom")
            .WithSummary("Creates a new room.")
            .WithDescription("Creates a new room with a unique display name and saves it to the database.")
            .Produces<RoomResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapGet("/{id:guid}", GetRoomByIdAsync)
            .WithName("GetRoomById")
            .WithSummary("Retrieves a room by ID.")
            .WithDescription("Retrieves a room by its unique ID. Returns 404 if the room does not exist.")
            .Produces<RoomResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<IResult> CreateRoomAsync(
        CreateRoomRequest request,
        IRoomRepository roomRepository,
        IUserRepository userRepository,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["Name"] = ["Name is required and cannot be whitespace."]
            });
        }

        var normalizedName = request.Name.Trim();
        var nameExists = await roomRepository.ExistsByNameAsync(normalizedName, cancellationToken);
        if (nameExists)
        {
            return Results.Conflict(new
            {
                Message = $"A room with the name '{normalizedName}' already exists."
            });
        }

        var hostUser = await userRepository.GetByIdAsync(request.HostUserId, cancellationToken);
        if (hostUser is null)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["HostUserId"] = ["The specified host user does not exist."]
            });
        }

        var room = new Room(Guid.NewGuid(), normalizedName, request.HostUserId, RoomStatus.Open, DateTime.UtcNow);
        await roomRepository.AddAsync(room, cancellationToken);

        var response = new RoomResponse(room.Id, room.Name, room.HostUserId, room.Status, room.CreatedUtc);
        return Results.Created($"/rooms/{room.Id}", response);
    }

    private static async Task<IResult> GetRoomByIdAsync(
        Guid id,
        IRoomRepository roomRepository,
        CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
        {
            return Results.NotFound(new
            {
                Message = $"No room found with ID '{id}'."
            });
        }

        var response = new RoomResponse(room.Id, room.Name, room.HostUserId, room.Status, room.CreatedUtc);
        return Results.Ok(response);
    }
}
