namespace Ticaga.Domain.Users;

/// <summary>
/// Represents a Ticaga platform user. Users can join rooms and participate in game sessions.
/// </summary>
public class User
{
    public Guid Id { get; private set; }

    /// <summary>
    /// The display name visible to other players. (Must be unique.)
    /// </summary>
    public string DisplayName { get; private set; }

    public DateTime CreatedUtc { get; private set; }

    public User(Guid id, string displayName, DateTime createdUtc)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(id));
        
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be null, empty, or whitespace.", nameof(displayName));
        
        if (createdUtc == default)
            throw new ArgumentException("CreatedUtc must be a valid UTC date/time.", nameof(createdUtc));

        if (createdUtc.Kind != DateTimeKind.Utc)
            throw new ArgumentException("CreatedUtc must be specified as UTC.", nameof(createdUtc));

        Id = id;
        DisplayName = displayName.Trim();
        CreatedUtc = createdUtc;
    }
}
