using Ticaga.Domain.Rooms;

namespace Ticaga.Application.Features.Rooms.GetRoomById.Dto;

public sealed record GetRoomByIdResult(
    Guid Id,
    string Name,
    Guid HostUserId,
    RoomStatus Status,
    DateTime CreatedUtc);
