using Ticaga.Domain.Users;

namespace Ticaga.Domain.Rooms;

public interface IRoomRepository
{
    Task<Room> AddAsync(Room room, CancellationToken cancellationToken = default);

    Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
