using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum MenuType
{
    TITLE,
    GAME
}

/**
 * UIManager loads and destroys menus. This class provides a strong base for a deterministic and well behaved menu
 * system.
 * 
 * Instead of having all your menus load with a scene they are loaded and destroyed as needed. This is a more scalable
 * solution and keeps load times short. When used properly there should only be one main scene with only a couple game
 * objects, UIManager being one of them.
 * 
 * This class must target one menu to load initially, and state + open/close calls should be managed by menus.
 */
public class UIManager : MonoBehaviour
{
    private MenuType BASE_PANEL = MenuType.TITLE;
    private MenuState currentState = null;
    private FiniteStateMachine<MenuState> fsm = null;

    private Stack<Tuple<MenuType, Menu>> panelStack = new Stack<Tuple<MenuType, Menu>>();

    void Start()
    {
        currentState = new TitleMenuState(new MenuState(this));

        fsm = new FiniteStateMachine<MenuState>(
            (int)BASE_PANEL,
            new List<Transition<MenuState>> {
                // Insert all possible menu transitions here and hook them up to the correct transition function.

                // Title
                new Transition<MenuState>(
                    (int)MenuType.TITLE,
                    (int)MenuType.GAME,
                    () => new GameMenuState((TitleMenuState)currentState)),

                // Game
                new Transition<MenuState>(
                    (int)MenuType.GAME,
                    (int)MenuType.TITLE,
                    () => new TitleMenuState((GameMenuState)currentState)),
            }
        );

        Menu menu = GetMenu(BASE_PANEL);
        panelStack.Push(new Tuple<MenuType, Menu>(BASE_PANEL, menu));
        menu.Open(currentState);
    }

    private Menu GetMenu(MenuType p)
    {
        string panelName = Enum.GetName(typeof(MenuType), p).ToLower();
        GameObject prefab = (GameObject)Resources.Load("Prefabs/UI/Menu/" + panelName);
        if (prefab == null)
        {
            Debug.LogError("Can't find panel with name: " + panelName);
        }

        GameObject panelObj = Generic.Instantiate(prefab);
        panelObj.transform.SetParent(this.transform, false);
        Menu panel = panelObj.GetComponent<Menu>();
        if (panel == null)
        {
            Debug.LogError(panelName + " doesn't have a panel component");
        }
        return panel;
    }

    public void OpenMenu(MenuType type)
    {
        MenuState nextState = fsm.Transition((int)type);
        if (nextState == null)
        {
            return;
        }
        currentState = nextState;

        Menu menu = GetMenu(type);
        if (menu == null)
        {
            return;
        }

        if (panelStack.Count > 0)
        {
            panelStack.Peek().second.Hide();
        }
        panelStack.Push(new Tuple<MenuType, Menu>(type, menu));
        menu.Open(currentState);
    }

    public void CloseMenu(MenuType p)
    {
        string panelName = Enum.GetName(typeof(MenuType), p).ToLower();
        Menu panel = panelStack.Peek().second;
        if (panel.name != panelName)
        {
            Debug.LogError(
                "You can only close panels at the top of the stack!" +
                string.Format("Current panel: {0}, panel to close: {1}", panel.name, panelName));
            return;
        }

        Tuple<MenuType, Menu> stackItem = panelStack.Pop();
        if (panelStack.Count > 0)
        {
            Tuple<MenuType, Menu> nextStackItem = panelStack.Peek();
            MenuState nextState = fsm.Transition((int)nextStackItem.first);
            if (nextState == null)
            {
                panelStack.Push(stackItem);
                return;
            }
            currentState = nextState;
            panel.Close();
            nextStackItem.second.Show(currentState);
        }
        else
        {
            panel.Close();
        }
    }
}
