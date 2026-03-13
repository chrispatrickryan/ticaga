using Microsoft.EntityFrameworkCore;
using Ticaga.Domain.Rooms;
using Ticaga.Infrastructure.Persistence;

namespace Ticaga.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly TicagaDbContext _dbContext;

    public RoomRepository(TicagaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room> AddAsync(Room room, 
        CancellationToken cancellationToken = default)
    {
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return room;
    }

    public async Task<Room?> GetByIdAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rooms
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rooms
            .AsNoTracking()
            .AnyAsync(r => r.Name == name, cancellationToken);
    }
}
