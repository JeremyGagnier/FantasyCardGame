using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature1 : Card
{
    override public bool isPermanent { get { return true; } }
    override public string name { get { return "Creature1"; } }
    override public byte colorlessManaCost { get { return 1; } }
    override public int attack { get { return 2; } }
    override public int health { get { return 2; } }
}
