using Microsoft.EntityFrameworkCore;
using Ticaga.Domain.Users;
using Ticaga.Infrastructure.Persistence;

namespace Ticaga.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly TicagaDbContext _dbContext;

    public UserRepository(TicagaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddAsync(User user, 
        CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByDisplayNameAsync(string username, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.DisplayName == username, cancellationToken);
    }
}
