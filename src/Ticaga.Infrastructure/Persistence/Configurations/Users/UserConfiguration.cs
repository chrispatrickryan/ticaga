using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticaga.Domain.Users;

namespace Ticaga.Infrastructure.Persistence.Configurations.Users;

public sealed class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.DisplayName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.NormalizedDisplayName)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.NormalizedDisplayName)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.CreatedUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasIndex(x => x.DisplayName)
            .IsUnique();
    }
}