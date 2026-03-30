namespace Ticaga.Domain.Users;

public interface IUserRepository
{
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByDisplayNameAsync(string displayName, CancellationToken cancellationToken = default);
}