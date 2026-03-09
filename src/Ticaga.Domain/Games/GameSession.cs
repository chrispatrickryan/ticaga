namespace Ticaga.Domain.Games;

public class GameSession
{
    public Guid Id { get; private set; }

    public Guid RoomId { get; private set; }

    public GameType GameType { get; private set; }

    public GameSessionState State { get; private set; }

    public Guid? CurrentTurnPlayerId { get; private set; }

    public DateTime CreatedUtc { get; private set; }

    public DateTime? StartedUtc { get; private set; }

    public DateTime? EndedUtc { get; private set; }

    public GameSession(
        Guid id,
        Guid roomId,
        GameType gameType,
        GameSessionState state,
        Guid? currentTurnPlayerId,
        DateTime createdUtc,
        DateTime? startedUtc,
        DateTime? endedUtc)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Game session ID cannot be empty.", nameof(id));

        if (roomId == Guid.Empty)
            throw new ArgumentException("Room ID cannot be empty.", nameof(roomId));

        if (currentTurnPlayerId.HasValue && currentTurnPlayerId.Value == Guid.Empty)
            throw new ArgumentException("Current turn player ID cannot be empty.", nameof(currentTurnPlayerId));

        if (createdUtc == default)
            throw new ArgumentException("CreatedUtc must be a valid UTC date/time.", nameof(createdUtc));

        if (createdUtc.Kind != DateTimeKind.Utc)
            throw new ArgumentException("CreatedUtc must be specified as UTC.", nameof(createdUtc));

        if (startedUtc.HasValue && startedUtc.Value == default)
            throw new ArgumentException("StartedUtc must be a valid UTC date/time.", nameof(startedUtc));

        if (startedUtc.HasValue && startedUtc.Value.Kind != DateTimeKind.Utc)
            throw new ArgumentException("StartedUtc must be specified as UTC.", nameof(startedUtc));

        if (endedUtc.HasValue && endedUtc.Value == default)
            throw new ArgumentException("EndedUtc must be a valid UTC date/time.", nameof(endedUtc));

        if (endedUtc.HasValue && endedUtc.Value.Kind != DateTimeKind.Utc)
            throw new ArgumentException("EndedUtc must be specified as UTC.", nameof(endedUtc));

        if (startedUtc.HasValue && startedUtc.Value < createdUtc)
            throw new ArgumentException("StartedUtc cannot be earlier than CreatedUtc.", nameof(startedUtc));

        if (endedUtc.HasValue && endedUtc.Value < createdUtc)
            throw new ArgumentException("EndedUtc cannot be earlier than CreatedUtc.", nameof(endedUtc));

        if (startedUtc.HasValue && endedUtc.HasValue && startedUtc > endedUtc)
            throw new ArgumentException("StartedUtc must be earlier than EndedUtc.", nameof(startedUtc));

        Id = id;
        RoomId = roomId;
        GameType = gameType;
        State = state;
        CurrentTurnPlayerId = currentTurnPlayerId;
        CreatedUtc = createdUtc;
        StartedUtc = startedUtc;
        EndedUtc = endedUtc;
    }
}
