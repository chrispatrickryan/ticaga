using Microsoft.EntityFrameworkCore;
using Ticaga.Domain.Games;
using Ticaga.Domain.Rooms;
using Ticaga.Domain.Users;

namespace Ticaga.Infrastructure.Persistence;

public sealed class TicagaDbContext : DbContext
{
    public TicagaDbContext(DbContextOptions<TicagaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<RoomMember> RoomMembers => Set<RoomMember>();

    public DbSet<User> Users => Set<User>();

    public DbSet<GameSession> GameSessions => Set<GameSession>();

    public DbSet<GamePlayer> GamePlayers => Set<GamePlayer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicagaDbContext).Assembly);
    }
}