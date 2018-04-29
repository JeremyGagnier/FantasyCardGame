using System.Collections.Generic;

public class Targets
{
    public List<int> players;
    public List<CardState> cards;

    public Targets(int player)
    {
        players = new List<int>(2);
        players.Add(player);
    }

    public Targets(CardState card)
    {
        cards = new List<CardState>(14);
        cards.Add(card);
    }
}
