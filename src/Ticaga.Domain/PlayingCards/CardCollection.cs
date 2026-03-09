
using System.Collections;

namespace Ticaga.Domain.PlayingCards;

/// <summary>
/// Encapsulation of a group of cards. Can represent a deck, discard pile, player's hand, etc.
/// </summary>
public class CardCollection : IEnumerable<PlayingCard>
{
    private readonly List<PlayingCard> _cards = new();
    private static readonly Random _random = new();

    public int Count => _cards.Count;

    public IReadOnlyList<PlayingCard> Cards => _cards;

    public bool IsEmpty => _cards.Count == 0;

    public void Add(PlayingCard card)
    {
        _cards.Add(card);
    }

    public void AddRange(IEnumerable<PlayingCard> cards)
    {
        _cards.AddRange(cards);
    }

    public PlayingCard Draw()
    {
        if (_cards.Count == 0)
        {
            throw new InvalidOperationException("Cannot draw from an empty collection.");
        }

        var card = _cards[^1];
        _cards.RemoveAt(_cards.Count - 1);
        return card;
    }

    public IReadOnlyList<PlayingCard> Draw(int count, bool allowFewer = false)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (!allowFewer && count > _cards.Count)
            throw new InvalidOperationException("Not enough cards to draw.");

        int cardsToDraw = allowFewer
            ? Math.Min(count, _cards.Count)
            : count;

        var drawn = new List<PlayingCard>(cardsToDraw);

        for (int i = 0; i < cardsToDraw; i++)
        {
            drawn.Add(Draw());
        }

        return drawn;
    }

    /// <summary>
    /// Fisher–Yates shuffle implementation.
    /// </summary>
    public void Shuffle()
    {
        for (int i = _cards.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }

    public void Sort()
    {
        _cards.Sort((a, b) =>
        {
            int suitComparison = a.Suit.CompareTo(b.Suit);
            return suitComparison != 0 ? suitComparison : a.Rank.CompareTo(b.Rank);
        });
    }

    public IEnumerator<PlayingCard> GetEnumerator() => _cards.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
