using Ticaga.Domain.Rooms;

namespace Ticaga.Application.Features.Rooms.CreateRoom.Dto;

public sealed record CreateRoomResult(
    Guid Id,
    string Name,
    Guid HostUserId,
    RoomStatus Status,
    DateTime CreatedUtc);
