using Ticaga.Domain.Rooms;

namespace Ticaga.Api.Rooms;

public sealed record RoomResponse(Guid Id, string Name, Guid HostUserId, RoomStatus Status, DateTime CreatedUtc);
