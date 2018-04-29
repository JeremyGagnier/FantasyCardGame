using System.Collections.Generic;

public class Hero
{
    public static Dictionary<string, Hero> byName = new Dictionary<string, Hero>()
    {
    };

    virtual public string name { get { return ""; } }
    virtual public int health { get { return 0; } }
}
