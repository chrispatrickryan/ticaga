namespace Ticaga.Domain.Rooms
{
    public class RoomMember
    {
        public Guid Id { get; private set; }

        public Guid RoomId { get; private set; }

        public Guid UserId { get; private set; }

        public bool IsHost { get; private set; }

        public bool IsReady { get; private set; }

        public DateTime JoinedUtc { get; private set; }

        public RoomMember(
            Guid id,
            Guid roomId,
            Guid userId,
            bool isHost,
            bool isReady,
            DateTime joinedUtc)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty.", nameof(id));

            if (roomId == Guid.Empty)
                throw new ArgumentException("Room ID cannot be empty.", nameof(id));

            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(id));

            if (joinedUtc == default)
                throw new ArgumentException("JoinedUtc must be a valid UTC date/time.", nameof(joinedUtc));

            if (joinedUtc.Kind != DateTimeKind.Utc)
                throw new ArgumentException("JoinedUtc must be specified as UTC.", nameof(joinedUtc));

            Id = id;
            RoomId = roomId;
            UserId = userId;
            IsHost = isHost;
            IsReady = isReady;
            JoinedUtc = joinedUtc;
        }
    }
}
