using System;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public readonly UIHooks uiHooks;

    private readonly Informer informer;
    private readonly BoardState board;

    private readonly int localPlayerNum;
    private readonly int oppPlayerNum;

    static public Game StartLocalGame(string localDeckName, string oppDeckName)
    {
        return new Game(new LocalInformer(localDeckName, oppDeckName), true);
    }

	public Game(Informer informer, bool doesLocalGoFirst)
    {
        uiHooks = new UIHooks();
        this.informer = informer;
        localPlayerNum = doesLocalGoFirst ? 0 : 1;
        oppPlayerNum = doesLocalGoFirst ? 1 : 0;
        this.board = new BoardState(
            informer.GetLocalHero(),
            informer.GetOppHero(),
            informer.GetLocalDeckSize(),
            informer.GetOppDeckSize(),
            doesLocalGoFirst ? GameRules.FIRST_PLAYER_RAMP : GameRules.SECOND_PLAYER_RAMP,
            doesLocalGoFirst ? GameRules.SECOND_PLAYER_RAMP : GameRules.FIRST_PLAYER_RAMP,
            doesLocalGoFirst);
    }

    public void Draw(int playerNum)
    {
        if (playerNum == localPlayerNum)
        {
            board.localHand.Add(new CardState(informer.DrawLocalCard(), localPlayerNum));
        }
        else
        {
            board.oppDeckSize -= 1;
            board.oppHandSize += 1;
        }
        foreach (CardState card in board.playOrder)
        {
            card.TryTrigger(EffectTrigger.DRAW, this, board, new Targets(playerNum));
        }
    }

    public void StartTurn()
    {
        int turnPlayer = board.turn % 2;
        if (board.ramp[turnPlayer] > 0)
        {
            board.ramp[turnPlayer] -= 1;
            board.colorlessMana[turnPlayer] += 1;
        }
        board.availableColorlessMana[turnPlayer] = board.colorlessMana[turnPlayer];
        board.availableBlueMana[turnPlayer] = board.blueMana[turnPlayer];
        board.availableRedMana[turnPlayer] = board.redMana[turnPlayer];
        board.availableGreenMana[turnPlayer] = board.greenMana[turnPlayer];
        board.availableBlackMana[turnPlayer] = board.blackMana[turnPlayer];

        Draw(turnPlayer);
        foreach (CardState card in board.playOrder)
        {
            card.TryTrigger(EffectTrigger.START_OF_TURN, this, board, new Targets(turnPlayer));
        }
    }

    public void EndTurn()
    {
        int turnPlayer = board.turn % 2;
        foreach (CardState card in board.playOrder)
        {
            card.TryTrigger(EffectTrigger.END_OF_TURN, this, board, new Targets(turnPlayer));
        }
        board.turn += 1;
    }

    public void CanPlay(CardState card, Targets targets)
    {
        card.CanPlay(board, targets);
    }

    public void Play(CardState card, Targets targets)
    {
        if (!card.CanPlay(board, targets))
        {
            Debug.LogError("The card '" + card.name + "' cannot be played with this board state!");
            return;
        }

        int turnPlayer = board.turn % 2;

        card.TryTrigger(EffectTrigger.PLAY, this, board, targets);

        foreach (CardState boardCard in board.playOrder)
        {
            boardCard.TryTrigger(EffectTrigger.CARD_PLAYED, this, board, new Targets(card));
        }

        if (card.isPermanent)
        {
            board.playOrder.Add(card);
            board.board[turnPlayer].Add(card);
        }
    }

    public void Attack(CardState attacker, Targets defender)
    {
        if (defender.cards.Count + defender.players.Count != 1)
        {
            Debug.LogError("There must be exactly one defender, found " +
                (defender.cards.Count + defender.players.Count).ToString());
            return;
        }

        Targets combatants = new Targets(attacker);
        if (defender.cards.Count == 0)
        {
            combatants.players.Add(defender.players[0]);
        }
        else
        {
            combatants.cards.Add(defender.cards[0]);
        }

        foreach (CardState boardCard in board.playOrder)
        {
            boardCard.TryTrigger(EffectTrigger.ATTACK, this, board, combatants);
        }
    }

    public void Damage(CardState card, int damage)
    {
        card.health -= damage;

        foreach (CardState boardCard in board.playOrder)
        {
            boardCard.TryTrigger(EffectTrigger.DAMAGED, this, board, new Targets(card));
        }

        if (card.health <= 0)
        {
            foreach (CardState boardCard in board.playOrder)
            {
                boardCard.TryTrigger(EffectTrigger.DESTROYED, this, board, new Targets(card));
            }
            board.playOrder.Remove(card);
            board.board[card.playerNum].Remove(card);
        }
    }

    public void Damage(int playerNum, int damage)
    {
        board.hero[playerNum].health -= damage;

        foreach (CardState boardCard in board.playOrder)
        {
            boardCard.TryTrigger(EffectTrigger.DAMAGED, this, board, new Targets(playerNum));
        }

        if (board.hero[playerNum].health <= 0)
        {
            GameOver(1 - playerNum);
        }
    }

    public void GameOver(int winner)
    {
        if (winner == localPlayerNum)
        {
            Debug.Log("You're winner!");
        }
        else
        {
            Debug.Log("You're a loose!");
        }
    }
}
