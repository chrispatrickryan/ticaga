namespace Ticaga.Domain.Games
{
    public class GamePlayer
    {
        public Guid Id { get; private set; }

        public Guid GameSessionId { get; private set; }

        public Guid UserId { get; private set; }

        public string DisplayName { get; private set; }

        public int? TeamNumber { get; private set; }

        public int SeatPosition { get; private set; }

        public int Score { get; private set; }

        public DateTime JoinedUtc { get; private set; }

        public GamePlayer(
            Guid id,
            Guid gameSessionId,
            Guid userId,
            string displayName,
            int? teamNumber,
            int seatPosition,
            int score,
            DateTime joinedUtc)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Game player ID cannot be empty.", nameof(id));

            if (gameSessionId == Guid.Empty)
                throw new ArgumentException("Game session ID cannot be empty.", nameof(gameSessionId));

            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Display name cannot be null, empty, or whitespace.", nameof(displayName));

            if (seatPosition <= 0)
                throw new ArgumentOutOfRangeException(nameof(seatPosition), "Seat position must be greater than zero.");

            if (teamNumber is <= 0)
                throw new ArgumentOutOfRangeException(nameof(teamNumber), "Team number must be greater than zero.");

            if (joinedUtc == default)
                throw new ArgumentException("JoinedUtc must be a valid UTC date/time.", nameof(joinedUtc));

            if (joinedUtc.Kind != DateTimeKind.Utc)
                throw new ArgumentException("JoinedUtc must be specified as UTC.", nameof(joinedUtc));

            Id = id;
            GameSessionId = gameSessionId;
            UserId = userId;
            DisplayName = displayName;
            TeamNumber = teamNumber;
            SeatPosition = seatPosition;
            Score = score;
            JoinedUtc = joinedUtc;
        }
    }
}
