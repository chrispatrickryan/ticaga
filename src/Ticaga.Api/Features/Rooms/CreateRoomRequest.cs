namespace Ticaga.Api.Features.Rooms;

public sealed record CreateRoomRequest(string Name, Guid HostUserId);
