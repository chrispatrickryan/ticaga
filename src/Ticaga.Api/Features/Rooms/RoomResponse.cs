using Ticaga.Domain.Rooms;

namespace Ticaga.Api.Features.Rooms;

public sealed record RoomResponse(Guid Id, string Name, Guid HostUserId, RoomStatus Status, DateTime CreatedUtc);
