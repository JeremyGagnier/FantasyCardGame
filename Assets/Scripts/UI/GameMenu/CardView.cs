using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CardView :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    private static float DRAG_TIME = 0.125f;
    private static float Y_GOAL = -420f;
    private static float X_GOAL_BASE = -280f;
    private static float X_CARD_SPACE = 200f;
    private static float X_CARD_SQUISH = 10f;

    public CardState card;
    private int handPosition;

    private Action<int> onHovered;
    private Action<int> onUnhovered;
    private Action<int> onPressed;
    private Action<int> onReleased;
    private bool held = false;
    private float heldTimer = 0f;
    public bool dragging = false;

    public void Setup(
        CardState card,
        int handPosition,
        Action<int> onHovered,
        Action<int> onUnhovered,
        Action<int> onPressed,
        Action<int> onReleased)
    {
        this.card = card;
        this.handPosition = handPosition;
        this.onHovered = onHovered;
        this.onUnhovered = onUnhovered;
        this.onPressed = onPressed;
        this.onReleased = onReleased;
    }

    public void SetHandPosition(int handPosition)
    {
        this.handPosition = handPosition;
    }

    public void Advance(int numberOfCards, int hoveredIndex)
    {
        if (held && !dragging)
        {
            heldTimer += Time.deltaTime;
            if (heldTimer > DRAG_TIME)
            {
                dragging = true;
            }
        }
        else
        {
            heldTimer = 0f;
        }

        if (dragging)
        {
            if (!IsInDeadzone((Vector2)transform.position, (Vector2)Input.mousePosition, 4f))
            {
                transform.position = new Vector3(
                    Mathf.Lerp(transform.position.x, Input.mousePosition.x, 0.3f),
                    Mathf.Lerp(transform.position.y, Input.mousePosition.y, 0.3f));
            }
        }
        else
        {
            float gap = X_CARD_SPACE - X_CARD_SQUISH * numberOfCards;
            float x_goal = X_GOAL_BASE + handPosition * gap;

            if (!IsInDeadzone((Vector2)transform.localPosition, new Vector2(x_goal, Y_GOAL), 4f))
            {
                transform.localPosition = new Vector3(
                    Mathf.Lerp(transform.localPosition.x, x_goal, 0.1f),
                    Mathf.Lerp(transform.localPosition.y, Y_GOAL, 0.1f));
            }
        }
    }

    public bool IsInDeadzone(Vector2 position, Vector2 goalPosition, float deadzoneSqr)
    {
        float xDiff = (position.x - goalPosition.x);
        float yDiff = (position.y - goalPosition.y);
        return (xDiff * xDiff + yDiff * yDiff) < deadzoneSqr;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        onHovered(handPosition);
    }

    public void OnPointerExit(PointerEventData e)
    {
        onUnhovered(handPosition);
    }

    public void OnPointerDown(PointerEventData e)
    {
        onPressed(handPosition);
        held = true;
    }

    public void OnPointerUp(PointerEventData e)
    {
        onReleased(handPosition);
        held = false;
        dragging = false;
    }
}
