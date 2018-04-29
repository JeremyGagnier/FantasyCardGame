public class TitleMenu : Menu
{
    private TitleMenuState state;
    public override void Open(MenuState state)
    {
        this.state = new TitleMenuState(state);

    }
}
