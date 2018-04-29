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

abstract public class Card : MonoBehaviour
{
    public static Dictionary<string, Card> byName = new Dictionary<string, Card>()
    {
        {"GainBlueMana", new GainBlueMana()}
    };

    abstract public bool isPermanent { get; }
    virtual public int attack { get { return 0; } }
    virtual public int health { get { return 0; } }
    virtual public int colorlessManaCost { get { return 0; } }
    virtual public int blueManaCost { get { return 0; } }
    virtual public int redManaCost { get { return 0; } }
    virtual public int greenManaCost { get { return 0; } }
    virtual public int blackManaCost { get { return 0; } }
    virtual public string effectString { get { return ""; } }

    virtual public bool CanTrigger(
        EffectTrigger trigger,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
        return true;
    }

    virtual public void Trigger(
        EffectTrigger trigger,
        Game game,
        BoardState boardState,
        Targets targets,
        int playerNum)
    {
    }
}
