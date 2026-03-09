using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ticaga.Infrastructure.Persistence.DesignTime;

public sealed class TicagaDbContextFactory : IDesignTimeDbContextFactory<TicagaDbContext>
{
    public TicagaDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("TicagaDatabase")
            ?? "Host=localhost;Port=5432;Database=ticaga_dev;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<TicagaDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new TicagaDbContext(optionsBuilder.Options);
    }
}
