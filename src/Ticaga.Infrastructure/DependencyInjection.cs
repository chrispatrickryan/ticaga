using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticaga.Domain.Rooms;
using Ticaga.Domain.Users;
using Ticaga.Infrastructure.Persistence;
using Ticaga.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TicagaDb")
            ?? throw new InvalidOperationException("Connection string 'TicagaDb' was not found.");

        services.AddDbContext<TicagaDbContext>(options =>
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention());

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();

        return services;
    }
}