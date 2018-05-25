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

    static public Game StartLocalGame(string localDeckName, string oppDeckName, UIHooks uiHooks)
    {
        return new Game(new LocalInformer(localDeckName, oppDeckName), true, uiHooks);
    }

	public Game(Informer informer, bool doesLocalGoFirst, UIHooks uiHooks)
    {
        this.uiHooks = uiHooks;
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

        for (int i = 0; i < GameRules.FIRST_PLAYER_STARTING_DRAW; ++i)
        {
            Draw(0);
        }
        for (int i = 0; i < GameRules.SECOND_PLAYER_STARTING_DRAW; ++i)
        {
            Draw(1);
        }
        StartTurn();
    }

    public void Draw(int playerNum)
    {
        if (playerNum == localPlayerNum)
        {
            CardState card = new CardState(informer.DrawLocalCard(), localPlayerNum);
            board.localHand.Add(card);
            uiHooks.onLocalDrawCard(card);
        }
        else
        {
            board.oppDeckSize -= 1;
            board.oppHandSize += 1;
            uiHooks.onOppDrawCard();
        }
        foreach (CardState card in board.playOrder)
        {
            card.TryTrigger(EffectTrigger.DRAW, this, board, new Targets(playerNum));
        }
    }

    public void EndTurn()
    {
        int turnPlayer = board.turn % 2;

        if (turnPlayer == localPlayerNum)
        {
            informer.EndTurn();
        }

        foreach (CardState card in board.playOrder)
        {
            card.TryTrigger(EffectTrigger.END_OF_TURN, this, board, new Targets(turnPlayer));
        }
        board.turn += 1;
        StartTurn();
    }

    public bool CanPlay(CardState card, Targets targets)
    {
        return card.CanPlay(board, targets);
    }

    public void Play(CardState card, Targets targets)
    {
        if (!card.CanPlay(board, targets))
        {
            Debug.LogError("The card '" + card.name + "' cannot be played with this board state!");
            return;
        }

        int turnPlayer = board.turn % 2;

        if (turnPlayer != card.playerNum)
        {
            Debug.LogError("Tried to play an opponents card!!");
            return;
        }

        if (turnPlayer == localPlayerNum)
        {
            informer.PlayCard(card, targets);
        }

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

    public void UseHeroActive(int playerNum, Targets targets)
    {
        int turnPlayer = board.turn % 2;
        if (turnPlayer != playerNum)
        {
            Debug.LogError("Tried activating opponents hero power!!");
            return;
        }
        informer.UseHeroActive(targets);
        board.hero[playerNum].TryActivate(this, board, targets);
    }

    public void Attack(CardState attacker, Targets defender)
    {
        if (defender.cards.Count + defender.players.Count != 1)
        {
            Debug.LogError("There must be exactly one defender, found " +
                (defender.cards.Count + defender.players.Count).ToString());
            return;
        }

        int turnPlayer = board.turn % 2;

        if (turnPlayer != attacker.playerNum)
        {
            Debug.LogError("Tried to attack with opponents creature!!");
            return;
        }

        Targets combatants = new Targets(attacker);
        if (defender.cards.Count == 0)
        {
            if (turnPlayer == defender.players[0])
            {
                Debug.LogError("Tried to attack themselves!");
                return;
            }
            combatants.players.Add(defender.players[0]);
        }
        else
        {
            if (turnPlayer == defender.cards[0].playerNum)
            {
                Debug.LogError("Tried to attack their own creature!");
                return;
            }
            combatants.cards.Add(defender.cards[0]);
        }

        informer.Attack(attacker, defender);

        foreach (CardState boardCard in board.playOrder)
        {
            boardCard.TryTrigger(EffectTrigger.ATTACK, this, board, combatants);
        }

        if (defender.cards.Count == 0)
        {
            Damage(defender.players[0], attacker.attack);
        }
        else
        {
            Damage(defender.cards[0], attacker.attack);
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

    private void StartTurn()
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

    private void GameOver(int winner)
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
