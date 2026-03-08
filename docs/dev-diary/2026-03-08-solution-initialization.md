# March 8, 2026 – Initial Ticaga setup: Git Repo and VS Solution Creation

## Git Repo

1. Created Ticaga repository on GitHub.
2. Edited README.md for Ticaga.
3. Cloned ticaga repo locally.
4. Updated .gitignore.
5. Added docs\dev-diary and diary files.
6. Added LICENSE file.
7. Created basic solution folder structure.

## Visual Studio API Solution

1. Created empty solution.
2. Added C# Ticaga.Domain project.
3. Added C# Ticaga.Infrastructure project.
4. Added ASP.NET Core Web API project.
5. Removed OpenApi in favor of Swagger.

# March 8, 2026 – Domain Model (First Pass)

## Domain Structure

Created initial domain namespaces and folders inside `Ticaga.Domain` to organize core platform concepts:

- `Games`
- `PlayingCards`
- `Rooms`
- `Users`

These represent the primary concepts required for a multiplayer card game platform.

## Users

Created the `User` domain entity.

**Purpose**
- Represents an authenticated Ticaga user
- Used for player identity in rooms and games

**Properties**
- `Id`
- `DisplayName`
- `CreatedUtc`

**Constructor validations**
- Non-empty `Guid`
- Non-empty display name
- UTC timestamps

## Rooms

Implemented the initial room/lobby model.

**Entities created**
- `Room`
- `RoomMember`
- `RoomStatus` (enum)

**Purpose**
- Represents a lobby where players gather before a game starts
- Supports tracking players currently in a room

**Room responsibilities**
- Room identity
- Host ownership
- Room state
- Membership tracking

## Games

Implemented the core game session model.

**Entities created**
- `GameSession`
- `GamePlayer`
- `GameSessionState` (enum)
- `GameType` (enum)

**Purpose**
- Represents an active or historical game played within a room

**Key responsibilities**
- Track players participating in a game
- Track current game state
- Track current player turn
- Track game lifecycle timestamps

`GameSession` constructor validations ensure:
- Valid GUID identifiers
- Valid UTC timestamps
- Logical ordering of `CreatedUtc`, `StartedUtc`, and `EndedUtc`

## Playing Cards

Implemented the reusable card model used by all card games.

**Entities created**
- `PlayingCard`
- `Suit` (enum)
- `Rank` (enum)

Ranks were assigned numeric values starting at `Two = 2` to support natural card comparisons.

Example:

```csharp
public enum Rank
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}
