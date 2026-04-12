namespace Ticaga.Api.Endpoints.Rooms.Dto;

public sealed record CreateRoomRequest(string Name, Guid HostUserId);
