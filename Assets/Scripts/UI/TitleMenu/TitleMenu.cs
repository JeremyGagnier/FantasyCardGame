using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : Menu
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button collectionButton;

    private TitleMenuState state;
    public override void Open(MenuState state)
    {
        this.state = new TitleMenuState(state);

        playButton.onClick.AddListener(OnPlay);
    }

    private void OnPlay()
    {
        state.mgr.CloseMenu(MenuType.TITLE);
        state.mgr.OpenMenu(MenuType.GAME);
    }
}
