namespace Ticaga.Domain.Rooms;

public class Room
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Guid HostUserId { get; private set; }

    public RoomStatus Status { get; private set; }

    public DateTime CreatedUtc { get; private set; }

    public Room(
        Guid id,
        string name,
        Guid hostUserId,
        RoomStatus status,
        DateTime createdUtc)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(name));

        if (hostUserId == Guid.Empty)
            throw new ArgumentException("Host user ID cannot be empty.", nameof(id));

        if (createdUtc == default)
            throw new ArgumentException("CreatedUtc must be a valid UTC date/time.", nameof(createdUtc));

        if (createdUtc.Kind != DateTimeKind.Utc)
            throw new ArgumentException("CreatedUtc must be specified as UTC.", nameof(createdUtc));

        Id = id;
        Name = name.Trim();
        HostUserId = hostUserId;
        Status = status;
        CreatedUtc = createdUtc;
    }
}
