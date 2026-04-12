using Ticaga.Application.Features.Rooms.GetRoomById.Dto;
using Ticaga.Domain.Rooms;

namespace Ticaga.Application.Features.Rooms.GetRoomById;

public sealed class GetRoomByIdHandler
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomByIdHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<OperationResult<GetRoomByIdResult>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
        {
            return OperationResult<GetRoomByIdResult>.NotFound($"Room with id {id} not found.");
        }

        return OperationResult<GetRoomByIdResult>.Success(
            new GetRoomByIdResult(
                room.Id, 
                room.Name, 
                room.HostUserId,
                room.Status,
                room.CreatedUtc));
    }
}
