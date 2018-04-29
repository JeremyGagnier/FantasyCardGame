public class HeroState
{
    public readonly Hero hero;
    public int health;

    public HeroState(Hero hero)
    {
        this.hero = hero;
        this.health = hero.health;
    }
}
