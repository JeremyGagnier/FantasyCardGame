using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameUIEvent
{
    LOCAL_DRAW,
    OPP_DRAW,

}

public class GameMenu : Menu
{
    private GameMenuState state;
    private Game game;
    private UIHooks uiHooks;

    [SerializeField] private Button endTurnButton;
    [SerializeField] private HeroView localHero;
    [SerializeField] private HeroView oppHero;

    private List<CardView> localHand = new List<CardView>();
    private List<CardView> oppHand = new List<CardView>();
    private CardView hoveredCard = null;

    public override void Open(MenuState state)
    {
        this.state = new GameMenuState(state);

        uiHooks = new UIHooks();
        uiHooks.onLocalDrawCard += OnLocalDrawCard;
        uiHooks.onOppDrawCard += OnOppDrawCard;
        game = Game.StartLocalGame("deck1", "deck2", uiHooks);
    }

    private void OnLocalDrawCard(CardState card)
    {
        GameObject cardObj = Generic.Instantiate(ResourceManager.card, transform);
        CardView cardView = cardObj.GetComponent<CardView>();
        cardView.Setup(
            card,
            localHand.Count,
            OnCardHovered,
            OnCardUnhovered,
            OnCardPressed,
            OnCardReleased);
        localHand.Add(cardView);
        cardObj.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
        cardObj.transform.localPosition = new Vector3(-750f, -680f, 0f);
    }

    private void OnCardHovered(int handPosition)
    {
        hoveredCard = localHand[handPosition];
    }

    private void OnCardUnhovered(int handPosition)
    {
        if (hoveredCard == localHand[handPosition])
        {
            hoveredCard = null;
        }
    }

    private void OnCardPressed(int handPosition)
    {
    }

    private void OnCardReleased(int handPosition)
    {
        if (localHand[handPosition].dragging && Input.mousePosition.y > 240)
        {
            CardState playedCard = localHand[handPosition].card;
            if (game.CanPlay(playedCard, null))
            {
                localHand.RemoveAt(handPosition);
                for (int i = handPosition; i < localHand.Count; ++i)
                {
                    localHand[i].SetHandPosition(i);
                }
                game.Play(playedCard, null);
            }
        }
    }

    private void OnOppDrawCard()
    {
    }

    private void Update()
    {
        foreach (CardView cardView in localHand)
        {
            cardView.Advance(localHand.Count, 0);
        }
    }
}
