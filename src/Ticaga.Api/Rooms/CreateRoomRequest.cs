namespace Ticaga.Api.Rooms;

public sealed record CreateRoomRequest(string Name, Guid HostUserId);
