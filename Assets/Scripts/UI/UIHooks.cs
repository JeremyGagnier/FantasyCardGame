using System;

public class UIHooks
{
    public Action<CardState> onLocalDrawCard = null;
    public void LocalDrawCard(CardState card)
    {
        if (onLocalDrawCard != null)
        {
            onLocalDrawCard(card);
        }
    }

    public Action onOppDrawCard = null;
    public void OppDrawCard()
    {
        if (onOppDrawCard != null)
        {
            onOppDrawCard();
        }
    }
}
