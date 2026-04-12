namespace Ticaga.Application.Features.Rooms.CreateRoom.Dto;

public sealed record CreateRoomCommand(string Name, Guid HostUserId);
