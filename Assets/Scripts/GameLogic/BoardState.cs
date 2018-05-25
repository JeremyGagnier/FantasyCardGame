using System.Collections.Generic;

public class BoardState
{
    public int turn = 0;

    public List<CardState> localHand = new List<CardState>();
    public int oppHandSize = 0;

    public List<CardState>[] board = {new List<CardState>(), new List<CardState>()};
    public List<CardState> playOrder = new List<CardState>();

    public byte[] colorlessMana = {0, 0};
    public byte[] redMana = {0, 0};
    public byte[] blueMana = {0, 0};
    public byte[] greenMana = {0, 0};
    public byte[] blackMana = {0, 0};
    public byte[] availableColorlessMana = {0, 0};
    public byte[] availableRedMana = {0, 0};
    public byte[] availableBlueMana = {0, 0};
    public byte[] availableGreenMana = {0, 0};
    public byte[] availableBlackMana = {0, 0};

    // These variables should be assigned
    public int localDeckSize;
    public int oppDeckSize;

    public HeroState[] hero = {null, null};

    public int[] ramp = {0, 0};

    public readonly bool doesLocalGoFirst;

    public BoardState(
        Hero localHero,
        Hero oppHero,
        int localDeckSize,
        int oppDeckSize,
        int localRamp,
        int oppRamp,
        bool doesLocalGoFirst)
    {
        this.hero[0] = new HeroState(localHero, 0);
        this.hero[1] = new HeroState(oppHero, 1);
        this.localDeckSize = localDeckSize;
        this.oppDeckSize = oppDeckSize;
        this.ramp[0] = doesLocalGoFirst ? localRamp : oppRamp;
        this.ramp[1] = doesLocalGoFirst ? oppRamp : localRamp;
        this.doesLocalGoFirst = doesLocalGoFirst;
    }
}
