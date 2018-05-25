using System;

abstract public class Informer
{
    // Info Fetching
    abstract public Hero GetLocalHero();
    abstract public Hero GetOppHero();
    abstract public Card DrawLocalCard();
    abstract public int GetOppHandSize();
    abstract public int GetLocalDeckSize();
    abstract public int GetOppDeckSize();

    // Local Actions
    abstract public void PlayCard(CardState card, Targets targets);
    abstract public void Attack(CardState attacker, Targets defender);
    abstract public void UseHeroActive(Targets targets);
    abstract public void EndTurn();

    // Opponent actions
    public Action<string, Targets> onCardPlayed = null;
    public Action<string, Targets> onAttacked = null;
    public Action<Targets> onUsedHeroActive = null;
    public Action onEndTurn = null;
}
