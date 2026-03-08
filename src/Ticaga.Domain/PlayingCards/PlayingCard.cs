namespace Ticaga.Domain.PlayingCards
{
    public record PlayingCard(Rank Rank, Suit Suit)
    {
        public override string ToString() => $"{Rank} of {Suit}";
    }
}
