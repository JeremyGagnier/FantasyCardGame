using UnityEngine;
using System.Collections.Generic;

public class ResourceManager
{
    private static string BASE_PATH = "Prefabs/";

    private static GameObject _card;
    public static GameObject card
    {
        get
        {
            if (_card == null)
            {
                _card = Resources.Load<GameObject>(BASE_PATH + "Card");
            }
            return _card;
        }
    }
}