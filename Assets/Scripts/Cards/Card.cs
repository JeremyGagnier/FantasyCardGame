using System;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTrigger
{
    PLAY,
    DEATH,
    END_OF_TURN,
    START_OF_TURN,
    DRAW,
    CARD_PLAYED,
    ATTACK,
    DAMAGED,
    DESTROYED
}

abstract public class Card
{
    public static Dictionary<string, Card> byName = new Dictionary<string, Card>()
    {
        {"GainBlueMana", new GainBlueMana()},
        {"Creature1", new Creature1()}
    };

    abstract public bool isPermanent { get; }
    abstract public string name { get; }
    virtual public int attack { get { return 0; } }
    virtual public int health { get { return 0; } }
    virtual public byte colorlessManaCost { get { return 0; } }
    virtual public byte blueManaCost { get { return 0; } }
    virtual public byte redManaCost { get { return 0; } }
    virtual public byte greenManaCost { get { return 0; } }
    virtual public byte blackManaCost { get { return 0; } }
    virtual public string effectString { get { return ""; } }

    virtual public bool CanTrigger(
        EffectTrigger trigger,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
        return boardState.availableColorlessMana[playerNum] >= colorlessManaCost &&
            boardState.availableBlueMana[playerNum] >= blueManaCost &&
            boardState.availableRedMana[playerNum] >= redManaCost &&
            boardState.availableGreenMana[playerNum] >= greenManaCost &&
            boardState.availableBlackMana[playerNum] >= blackManaCost;
    }

    virtual public void Trigger(
        EffectTrigger trigger,
        Game game,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
        boardState.availableColorlessMana[playerNum] -= colorlessManaCost;
        boardState.availableBlueMana[playerNum] -= blueManaCost;
        boardState.availableRedMana[playerNum] -= redManaCost;
        boardState.availableGreenMana[playerNum] -= greenManaCost;
        boardState.availableBlackMana[playerNum] -= blackManaCost;
    }
}
