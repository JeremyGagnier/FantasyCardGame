using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : Menu
{
    private GameMenuState state;

    private Game game;
    public override void Open(MenuState state)
    {
        this.state = new GameMenuState(state);

        game = Game.StartLocalGame("deck1", "deck2");
    }
}
