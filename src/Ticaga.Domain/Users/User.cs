namespace Ticaga.Domain.Users;

/// <summary>
/// Represents a Ticaga platform user. Users can join rooms and participate in game sessions.
/// </summary>
public sealed class User
{
    public Guid Id { get; private set; }

    /// <summary>
    /// Email is always stored trimmed and lowercase.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// The display name visible to other players. (Must be unique.)
    /// </summary>
    public string DisplayName { get; private set; } = string.Empty;

    public string NormalizedDisplayName { get; private set; } = string.Empty;

    /// <summary>
    /// Secure password hash for authentication.
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;

    public DateTime CreatedUtc { get; private set; }

    private User()
    {
        // Required by EF Core.
    }

    private User(Guid id, string email, string displayName, string passwordHash, DateTime createdUtc)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name is required.", nameof(displayName));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        if (createdUtc == default)
            throw new ArgumentException("CreatedUtc must be a valid UTC date/time.", nameof(createdUtc));

        if (createdUtc.Kind != DateTimeKind.Utc)
            throw new ArgumentException("CreatedUtc must be specified as UTC.", nameof(createdUtc));

        Id = id;
        Email = CanonicalizeEmail(email);
        DisplayName = displayName.Trim();
        NormalizedDisplayName = NormalizeDisplayName(displayName);
        PasswordHash = passwordHash;
        CreatedUtc = createdUtc;
    }

    public static User Register(string email, string displayName, DateTime? createdUtc)
    {
        return new User(
            Guid.NewGuid(),
            email,
            displayName,
            passwordHash: "__pending_hash__",
            createdUtc ?? DateTime.UtcNow);
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        }

        PasswordHash = passwordHash;
    }

    public static string CanonicalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        return email.Trim().ToLowerInvariant();
    }

    public static string NormalizeDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Display name is required.", nameof(displayName));
        }

        return displayName.Trim().ToLowerInvariant();
    }
}
