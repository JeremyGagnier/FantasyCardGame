using System.Collections.Generic;

/**
 *  CardState is used to manage the changeable values of cards, which are immutable.
 */
public class CardState
{
    private readonly Card card;
    public string name { get { return card.name; } }
    public bool isPermanent { get { return card.isPermanent; } }

    public int playerNum;
    public int health;

    public CardState(Card card, int playerNum)
    {
        this.card = card;
        this.playerNum = playerNum;
        this.health = card.health;
    }

    public void TryTrigger(
        EffectTrigger trigger,
        Game game,
        BoardState boardState,
        Targets targets)
    {
        if (card.CanTrigger(trigger, boardState, targets, playerNum))
        {
            card.Trigger(trigger, game, boardState, targets, playerNum);
        }
    }

    public bool CanPlay(BoardState boardState, Targets targets)
    {
        return card.CanTrigger(EffectTrigger.PLAY, boardState, targets, playerNum);
    }
}
