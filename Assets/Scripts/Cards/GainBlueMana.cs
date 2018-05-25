using System.Collections.Generic;

public class GainBlueMana : Card
{
    override public bool isPermanent { get { return false; } }
    override public string name { get { return "GainBlueMana"; } }
    override public string effectString { get { return "Convert one colorless mana into blue mana"; } }

    override public bool CanTrigger(
        EffectTrigger trigger,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
        switch (trigger)
        {
            case EffectTrigger.PLAY:
                return boardState.colorlessMana[playerNum] > 0;
            default:
                return false;
        }
    }

    override public void Trigger(
        EffectTrigger trigger,
        Game game,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
        boardState.colorlessMana[playerNum] -= 1;
        boardState.blueMana[playerNum] += 1;
    }
}
