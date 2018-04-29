using System.Collections.Generic;

public class LocalInformer : Informer
{
    private readonly Hero localHero;
    private readonly Hero oppHero;
    private readonly List<Card> localCards;
    private readonly List<Card> oppCards;

    public LocalInformer(string localDeckName, string oppDeckName)
    {
        DeckFile localDeck = new DeckFile(localDeckName);
        DeckFile oppDeck = new DeckFile(oppDeckName);
        localHero = localDeck.hero; // TODO: Shuffle
        localCards = localDeck.cards;
        oppHero = oppDeck.hero;
        oppCards = oppDeck.cards;
    }

    override public Hero GetLocalHero()
    {
        return localHero;
    }

    override public Hero GetOppHero()
    {
        return oppHero;
    }

    override public Card DrawLocalCard()
    {
        int lastCardAt = localCards.Count - 1;
        Card lastCard = localCards[lastCardAt];
        localCards.RemoveAt(lastCardAt);
        return lastCard;
    }

    override public int GetOppHandSize()
    {
        return 0;   // TODO: Figure out how to make this more robust
    }

    override public int GetLocalDeckSize()
    {
        return localCards.Count;
    }

    override public int GetOppDeckSize()
    {
        return oppCards.Count;
    }
}
