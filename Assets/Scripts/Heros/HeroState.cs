public class HeroState
{
    private readonly Hero hero;
    public readonly int playerNum;
    public int health;

    public HeroState(Hero hero, int playerNum)
    {
        this.hero = hero;
        this.playerNum = playerNum;
        this.health = hero.health;
    }

    public void TryPassivate(
       EffectTrigger trigger,
       Game game,
       BoardState boardState,
       Targets targets)
    {
        if (hero.CanPassivate(trigger, boardState, targets, playerNum))
        {
            hero.Passivate(trigger, game, boardState, targets, playerNum);
        }
    }

    public void TryActivate(
       Game game,
       BoardState boardState,
       Targets targets)
    {
        if (hero.CanActivate(boardState, targets, playerNum))
        {
            hero.Activate(game, boardState, targets, playerNum);
        }
    }
}
