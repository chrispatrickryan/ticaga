using Ticaga.Domain.Rooms;

namespace Ticaga.Api.Endpoints.Rooms.Dto;

public sealed record RoomSummaryResponse(Guid Id,
    string Name,
    Guid HostUserId,
    string Status,
    DateTime CreatedUtc);