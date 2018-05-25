using System.Collections.Generic;

public abstract class Hero
{
    public static Dictionary<string, Hero> byName = new Dictionary<string, Hero>()
    {
        {"Hero1", new Hero1()},
    };

    abstract public string name { get; }
    abstract public int health { get; }

    virtual public bool CanPassivate(
        EffectTrigger trigger,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
        return true;
    }

    virtual public void Passivate(
        EffectTrigger trigger,
        Game game,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
    }

    virtual public bool CanActivate(
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
        return true;
    }

    virtual public void Activate(
        Game game,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
    }
}
