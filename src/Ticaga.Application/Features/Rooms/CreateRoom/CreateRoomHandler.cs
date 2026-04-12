using Ticaga.Application.Common;
using Ticaga.Application.Features.Rooms.CreateRoom.Dto;
using Ticaga.Domain.Rooms;
using Ticaga.Domain.Users;

namespace Ticaga.Application.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;

    public CreateRoomHandler(IRoomRepository roomRepository, IUserRepository userRepository)
    {
        _roomRepository = roomRepository;
        _userRepository = userRepository;
    }

    public async Task<OperationResult<CreateRoomResult>> HandleAsync(
        CreateRoomCommand command,
        CancellationToken cancellationToken)
    {
        var normalizedName = string.IsNullOrWhiteSpace(command.Name) ? null : command.Name.Trim();
        if (normalizedName is null)
        {
            return OperationResult<CreateRoomResult>.ValidationProblem(new Dictionary<string, string[]>
            {
                ["Name"] = ["Name is required and cannot be whitespace."]
            });
        }

        var nameExists = await _roomRepository.ExistsByNameAsync(normalizedName, cancellationToken);
        if (nameExists)
        {
            return OperationResult<CreateRoomResult>
                .Conflict($"A room with the name '{normalizedName}' already exists.");
        }

        var hostUser = await _userRepository.GetByIdAsync(command.HostUserId, cancellationToken);
        if (hostUser is null)
        {
            return OperationResult<CreateRoomResult>.ValidationProblem(
                ValidationHelper.For("HostUserId", "The specified host user does not exist."));
        }

        var room = new Room(
            Guid.NewGuid(),
            normalizedName,
            command.HostUserId,
            RoomStatus.Open,
            DateTime.UtcNow);

        await _roomRepository.AddAsync(room, cancellationToken);

        return OperationResult<CreateRoomResult>.Success(
            new CreateRoomResult(
                room.Id,
                room.Name,
                room.HostUserId,
                room.Status,
                room.CreatedUtc));
    }
}
